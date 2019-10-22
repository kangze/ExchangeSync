using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using Microsoft.AspNetCore.Http;

namespace ExchangeSync.Models.Inputs
{
    public class AppointMenInput
    {
        /// <summary>
        /// 会议/约会 的主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容信息
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string Start { get; set; }

        public string StartTime { get; set; }

        public bool AddToSkype { get; set; }

        public bool FullDay { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string End { get; set; }

        public string EndTime { get; set; }

        /// <summary>
        /// 约会的地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 在多久之前提醒
        /// </summary>
        public DateTime? ReminderDueBy { get; set; }

        /// <summary>
        /// 通知人员,如果是约会的话,不需要处理这个属性
        /// </summary>
        public List<string> Attendees { get; set; }

        /// <summary>
        /// 会议的类型
        /// </summary>
        public AppointMentType Type { get; set; }

        public List<AttachmentInput> Attachments { get; set; }

        public List<IFormFile> Attachment { get; set; }
    }
}
