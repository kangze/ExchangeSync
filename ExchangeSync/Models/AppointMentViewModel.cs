using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public class AppointMentViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// 月的多少号
        /// </summary>
        [JsonProperty("day")]
        public int Day { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        [JsonProperty("week")]
        public int Week { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("attendees")]
        public List<AppointMentAttendeesViewModel> Attendees { get; set; }
    }

    public class AppointMentAttendeesViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("attende")]
        public string Attende { get; set; }
    }
}
