using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Models.Inputs
{
    public class NewMailInput
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        /// <summary>
        /// 邮件目的方
        /// </summary>
        public string[] Recivers { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] CarbonCopy { get; set; }
    }
}
