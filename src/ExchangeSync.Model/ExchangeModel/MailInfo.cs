using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.ExchangeModel
{
    public class MailInfo
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTimeOffset SendedTime { get; set; }
    }
}
