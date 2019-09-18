using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public class CreateMeetingResult
    {
        public string OnlineMeetingId { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string JoinUrl { get; set; }
    }
}
