using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public class AppointMentViewModel
    {
        public string Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Location { get; set; }

        public DateTime Start { get; set; }

        /// <summary>
        /// 月的多少号
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public int Week { get; set; }

        public DateTime End { get; set; }

        public List<AppointMentAttendeesViewModel> Attendees { get; set; }
    }

    public class AppointMentAttendeesViewModel
    {
        public string Name { get; set; }

        public string Attende { get; set; }
    }
}
