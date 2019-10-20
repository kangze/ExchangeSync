using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Exchange.Model
{
    public class AppointMentDto
    {
        /// <summary>
        /// 会议/约会 的主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容信息
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
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
    }

    public enum AppointMentType
    {
        /// <summary>
        /// 约会
        /// </summary>
        Date = 0,

        /// <summary>
        /// 会议
        /// </summary>
        Talk = 1
    }
}
