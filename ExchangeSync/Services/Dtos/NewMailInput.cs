using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services.Dtos
{
    public class NewMailInput
    {
        /// <summary>
        /// 发送方账号
        /// </summary>
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string[] Recivers { get; set; }

        public string[] CarbonCopy { get; set; }
    }
}
