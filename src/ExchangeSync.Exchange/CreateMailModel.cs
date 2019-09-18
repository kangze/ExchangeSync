using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Exchange
{
    public class CreateMailModel
    {
        /// <summary>
        /// 接收方
        /// </summary>
        public string TargetMail { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Body { get; set; }
    }
}
