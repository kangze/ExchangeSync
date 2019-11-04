using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExchangeSync.Services.Dtos
{
    public class CreateMeetingInput
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string Location { get; set; }

        public bool AllDay { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool OnLine { get; set; }

        public string ReminderDueBy { get; set; }

        public string[] Attendees { get; set; }

        public IList<IFormFile> Attachments { get; set; }
    }
}
