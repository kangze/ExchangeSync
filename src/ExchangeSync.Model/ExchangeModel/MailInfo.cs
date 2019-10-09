using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.ExchangeModel
{
    public class MailInfo
    {
        public string Id { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 是否被读取了
        /// </summary>
        public bool Readed { get; set; }

        /// <summary>
        /// 发送者的名字
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 发送者的邮件地址
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 接收日期
        /// </summary>
        public DateTimeOffset RecivedTime { get; set; }

        public List<AttachmentInfo> Attachments { get; set; }
    }

    public class AttachmentInfo
    {

        /// <summary>
        /// 特质附件的Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 对应邮件的Id
        /// </summary>
        public string MailId { get; set; }

        /// <summary>
        /// 附件的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大小,bytes
        /// </summary>
        public long Size { get; set; }
    }
}
