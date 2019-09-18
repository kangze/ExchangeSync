using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public class MailMessageViewModel
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public DateTimeOffset SendedTime { get; set; }
    }
}
