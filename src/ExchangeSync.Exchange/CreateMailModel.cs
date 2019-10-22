using System;
using System.Collections.Generic;
using System.IO;
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
        public string[] TargetMail { get; set; }

        public string[] Cc { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentMailModel> Attachments { get; set; }


    }

    public class AttachmentMailModel
    {
        public string Id { get; set; }

        public byte[] Bytes { get; set; }

        public string Name { get; set; }

        public bool IsPackage { get; set; }
    }
}
