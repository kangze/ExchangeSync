﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeSync.Models
{
    public class MailItemViewModel
    {
        [JsonProperty("mailId")]
        public string MailId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("sender")]
        public MailContactViewModel Sender { get; set; }

        [JsonProperty("recivers")]
        public List<MailContactViewModel> Recivers { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("readed")]
        public bool Readed { get; set; }
    }
}
