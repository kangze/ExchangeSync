using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Exchange.Model
{
    public class AppointMentDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 会议/约会 的主题
        /// </summary>
        [JsonProperty("title")]
        public string Subject { get; set; }

        [JsonProperty("mailId")]
        public string MailId { get; set; }

        /// <summary>
        /// 内容信息
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// 约会的地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 在多久之前提醒
        /// </summary>
        public DateTimeOffset? ReminderDueBy { get; set; }

        /// <summary>
        /// 通知人员,如果是约会的话,不需要处理这个属性
        /// </summary>
        public List<string> Attendees { get; set; }

        /// <summary>
        /// 会议的类型
        /// </summary>
        public AppointMentType Type { get; set; }

        public bool FullDay { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentMailModel> Attachments { get; set; }
    }

    public enum AppointMentType
    {
        /// <summary>
        /// 会议
        /// </summary>
        Talk = 0,
        /// <summary>
        /// 约会
        /// </summary>
        Date = 1,

    }
}
