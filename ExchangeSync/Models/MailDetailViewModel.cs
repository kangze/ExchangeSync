using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeSync.Models
{
    public class MailDetailViewModel
    {
        [JsonProperty("mailId")]
        public string MailId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("sender")]
        public MailContactViewModel Sender { get; set; }

        [JsonProperty("recivers")]
        public List<MailContactViewModel> Recivers { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonProperty("attachments")]
        public List<MailAttachmentViewModel> Attachments { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("folderName")]
        public string FolderName { get; set; }
    }
}
