using System;
using System.Collections.Generic;
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
    }

    public class MailContactInput
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
