using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Model;
using ExchangeSync.Model.EnterpiseContactModel;
using ExchangeSync.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses;
using Task = System.Threading.Tasks.Task;

namespace ExchangeSync.Extension
{
    public class MessageService
    {
        public MailManager MailManager { get; set; }

        public PullSubscription Pull { get; set; }

        public string Number { get; set; }
    }

    public static class NotiMax
    {
        public static int Current { get; set; }

        public static int SubScriptionMax { get; set; }
        public static int UserMax { get; set; }
        public static List<int> MailManagerMax { get; set; } = new List<int>();
    }

    public static class ServiceCollectionExtension
    {
        public static DbContextOptions<ServiceDbContext> Test { get; set; }
        //public static bool subThread { get; set; }
        public static IServiceCollection AddNofiyTask(this IServiceCollection services)
        {
            var dbOption = services.BuildServiceProvider().GetService<DbContextOptions<ServiceDbContext>>();
            Test = dbOption;
            var oaService = services.BuildServiceProvider().GetService<IOaSystemOperationService>();
            var userProfile = GetUserProfiles(dbOption).GetAwaiter().GetResult();
            userProfile.Add(new UserProfile() { Number = "111", Password = "tfs4418000", UserName = "v-ms-kz" });


            ThreadPool.QueueUserWorkItem(async _ =>
            {
                var subScriptions = new List<MessageService>();

                Parallel.ForEach(userProfile, async u =>
                {
                    try
                    {
                        var maileManager = new MailManager(u.UserName + "@scrbg.com", u.Password);
                        NotiMax.MailManagerMax.Add(1);
                        var subScription = await maileManager._exchangeService.SubscribeToPullNotifications(
                            new FolderId[] { WellKnownFolderName.Inbox }, 1440, null, EventType.NewMail);
                        subScriptions.Add(new MessageService() { MailManager = maileManager, Pull = subScription, Number = u.Number });
                    }
                    catch (Exception e)
                    {

                    }

                });

                while (true)
                {
                    Parallel.ForEach(subScriptions, async subScription =>
                    {
                        var events = subScription.Pull.GetEvents().GetAwaiter().GetResult();
                        IEnumerable<ItemEvent> itemEvents = events.ItemEvents;
                        foreach (ItemEvent itemEvent in itemEvents)
                        {
                            if (itemEvent.EventType == EventType.NewMail)
                            {
                                //发送新的到OA系统
                                EmailMessage emailMessage = await EmailMessage.Bind(subScription.MailManager._exchangeService, itemEvent.ItemId.UniqueId);
                                var mailId = itemEvent.ItemId.UniqueId;
                                var subject = emailMessage.Subject;
                                var number = subScription.Number;

                                var logStr = "mailId:" + mailId + "-subject:" + subject + "-number" + number;
                                try
                                {
                                    var result = await oaService.SendNewMailSync(mailId, subject, number);
                                    logStr += result;
                                }
                                catch (Exception e)
                                {
                                    logStr += e.ToString();
                                }

                                using (var db = new ServiceDbContext(Test))
                                {
                                    db.NewMailEvents.Add(new NewMailEvent()
                                    {
                                        NewMailId = "14342",
                                        Notify = false,
                                        Title = number,
                                        TextBody = logStr,
                                        DateTime = DateTime.Now
                                    });
                                    db.SaveChanges();
                                }
                            }
                        }
                    });
                    Thread.Sleep(5000);
                }
            });


            //ThreadPool.QueueUserWorkItem(async _ =>
            //{
            //    while (true)
            //    {
            //        var userProfile = await GetUserProfiles(dbOption);
            //        userProfile.Add("v-ms-kz", "tfs4418000");
            //        NotiMax.UserMax = userProfile.Count;
            //        List<PullSubscription> pulls = new List<PullSubscription>();
            //        foreach (var profile in userProfile)
            //        {
            //            try
            //            {
            //                var maileManager = MailManager.Create(profile.Key + "@scrbg.com", profile.Value);
            //                NotiMax.MailManagerMax++;
            //                var subScription = await maileManager._exchangeService.SubscribeToPullNotifications(
            //                    new FolderId[] { WellKnownFolderName.Inbox }, 1440, null, EventType.NewMail);
            //                pulls.Add(subScription);
            //            }
            //            catch (Exception)
            //            {

            //            }

            //        }

            //        NotiMax.SubScriptionMax = pulls.Count;
            //        ThreadPool.QueueUserWorkItem(async x =>
            //        {
            //            while (true)
            //            {
            //                foreach (var subscription in pulls)
            //                {
            //                    NotiMax.Current++;
            //                    var events = subscription.GetEvents().GetAwaiter().GetResult();
            //                    IEnumerable<ItemEvent> itemEvents = events.ItemEvents;
            //                    foreach (ItemEvent itemEvent in itemEvents)
            //                    {
            //                        if (itemEvent.EventType == EventType.NewMail)
            //                        {
            //                            //发送新的到OA系统
            //                            using (var db = new ServiceDbContext(Test))
            //                            {
            //                                db.NewMailEvents.Add(new NewMailEvent()
            //                                {
            //                                    NewMailId = "14342",
            //                                    Notify = false,
            //                                    Title = "11",
            //                                    TextBody = "111",
            //                                    DateTime = DateTime.Now
            //                                });
            //                                db.SaveChanges();
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //        });
            //        Thread.Sleep(1000 * 60 * 60 * 24);
            //    }
            //});
            return services;
        }

        public static async Task<List<UserProfile>> GetUserProfiles(DbContextOptions<ServiceDbContext> options)
        {
            using (var db = new ServiceDbContext(options))
            {
                var users = await (from auth in db.EmployeeAuths
                                   from employee in db.Employees
                                   where employee.Number == auth.Number
                                   select new UserProfile()
                                   {
                                       UserName = employee.UserName,
                                       Password = auth.Password,
                                       Number = employee.Number
                                   }).ToListAsync();
                var result = new Dictionary<string, string>();
                var users1 = new List<UserProfile>();
                foreach (var user in users)
                {
                    if (string.IsNullOrEmpty(user.UserName) || result.ContainsKey(user.UserName)) continue;
                    result.Add(user.UserName, user.Password.DecodeBase64());
                    users.Add(new UserProfile()
                    {
                        Number = user.Number,
                        UserName = user.UserName,
                        Password = user.Password.DecodeBase64()
                    });
                }

                return users1;
            }
        }
    }

    public class UserProfile
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Number { get; set; }
    }
}
