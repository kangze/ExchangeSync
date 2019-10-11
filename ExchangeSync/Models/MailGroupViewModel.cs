using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public class MailGroupViewModel
    {
        /// <summary>
        /// 分组名称:通常以时间来进行分类
        /// </summary>
        [JsonProperty("groupTitle")]
        public string GroupTitle { get; set; }

        /// <summary>
        /// 数据详情
        /// </summary>
        [JsonProperty("items")]
        public List<MailItemViewModel> Items { get; set; }
    }
}
