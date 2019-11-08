using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeSync.Model;
using ExchangeSync.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
                    using (var db = new ServiceDbContext(dbOption))
                    {
                        //var notifies = db.NewMailEvents.Where(u => !u.Notify).ToList();
                        //if (notifies.Count != 0)
                        //{
                        //    //Notify to Wechat!
                        //    foreach(var item in notifies)
                        //    {
                        //        await oaService.CreateAppointmentAsync(new Services.Dtos.OAAppoinmentInputDto()
                        //        {
                        //            First=item.Title,

                        //        })
                        //    }
                        //}
                    }
                    Thread.Sleep(5000);
                }
            });
            return services;
        }
    }
}
