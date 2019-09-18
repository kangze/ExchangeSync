using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ExchangeSyncHost.Internal;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeSyncHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new MailManager("v-ms-kz@scrbg.com", "tfs4418000");
            manager.GetMailMessage();
            //manager.SendMail(new CreateMailModel()
            //{
            //    Body = "测试body",
            //    Subject = "主题",
            //    TargetMail = "374187303@qq.com"
            //});
            Console.ReadKey();
        }
    }
}
