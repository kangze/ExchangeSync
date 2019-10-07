using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public class MailIndexViewModel
    {
        /// <summary>
        /// 分组名称:通常以时间来进行分类
        /// </summary>
        [JsonProperty("groupTitle")]
        public string GroupTitle { get; set; }

        [JsonProperty("items")]
        public List<MailIndexItemViewModel> Items { get; set; }
    }

    public class MailIndexItemViewModel
    {
        [JsonProperty("mailId")]
        public string MailId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 用于展示的名称
        /// </summary>
        [JsonProperty("senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// 真实的用户的电子邮件地址
        /// </summary>
        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("readed")]
        public bool Readed { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}
