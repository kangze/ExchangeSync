using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeSync.Models.Inputs
{
    public class ReplyMailInput
    {
        public string MailId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string[] Reciver { get; set; }

        public string[] CopyTo { get; set; }

        public List<AttachmentInput> Attachments { get; set; }

    }

    public class AttachmentInput
    {
        public string Id { get; set; }

        public byte[] Bytes { get; set; }

        public string Name { get; set; }
    }

    public class MailContactInput
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
