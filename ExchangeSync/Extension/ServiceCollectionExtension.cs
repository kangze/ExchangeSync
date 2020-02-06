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
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNofiyTask(this IServiceCollection services)
        {
            var dbOption = services.BuildServiceProvider().GetService<DbContextOptions<ServiceDbContext>>();
            var oaService = services.BuildServiceProvider().GetService<IOaSystemOperationService>();
            ThreadPool.QueueUserWorkItem(async _ =>
            {
                while (true)
                {
                    var userProfile = await GetUserProfiles(dbOption);

                    foreach (var profile in userProfile)
                    {
                        var maileManager = MailManager.Create(profile.Key + "@scrbg.com", profile.Value);
                        var subScription = await maileManager._exchangeService.SubscribeToPullNotifications(
                            new FolderId[] { WellKnownFolderName.Inbox }, 1440, null, EventType.NewMail);
                        var time = new Timer(TimeCallBack, subScription, TimeSpan.Zero, TimeSpan.FromSeconds(5));
                    }
                }
            });
            return services;
        }

        public static void TimeCallBack(object state)
        {
            PullSubscription sub = state as PullSubscription;
            var events = sub.GetEvents().GetAwaiter().GetResult();
            IEnumerable<ItemEvent> itemEvents = events.ItemEvents;
            foreach (ItemEvent itemEvent in itemEvents)
            {
                if (itemEvent.EventType == EventType.NewMail)
                {
                    //发送新的到OA系统

                }
            }
        }

        public static async Task<Dictionary<string, string>> GetUserProfiles(DbContextOptions<ServiceDbContext> options)
        {
            using (var db = new ServiceDbContext(options))
            {
                var users = await (from auth in db.EmployeeAuths
                                   from employee in db.Employees
                                   where employee.Number == auth.Number
                                   select new UserProfile()
                                   {
                                       UserName = employee.UserName,
                                       Password = auth.Password
                                   }).ToListAsync();
                var result = new Dictionary<string, string>();
                foreach (var user in users)
                {
                    if (result.ContainsKey(user.UserName)) continue;
                    result.Add(user.UserName, user.Password.DecodeBase64());
                }

                return result;
            }
        }
    }

    public class UserProfile
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
