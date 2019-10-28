using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class NewMailEvent
    {
        public Guid Id { get; set; }

        public string NewMailId { get; set; }

        public string Title { get; set; }

        public string TextBody { get; set; }

        /// <summary>
        /// 邮件接收得时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 标注是否被通知过
        /// </summary>
        public bool Notify { get; set; }
    }
}
