using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services.Dtos
{
    public class OAAppoinmentInputDto
    {
        public string MeetId { get; set; }

        public string UserNum { get; set; }

        /// <summary>
        /// 应该是一个标题
        /// </summary>
        public string First { get; set; }

        public string Keyword1 { get; set; }

        public string Keyword2 { get; set; }

        public string Keyword3 { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// Wechat消息模板Id可以不写
        /// </summary>
        public string TempleteIdStr { get; set; }
    }
}
