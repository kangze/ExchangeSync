using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model
{
    public class UserWeChat
    {
        /// <summary>
        /// MdmId
        /// </summary>
        [Key]
        public Guid MdmId { get; set; }

        /// <summary>
        /// SSo User Id
        /// </summary>
        public Guid SsoId { get; set; }

        /// <summary>
        /// wechat OpenId
        /// </summary>
        public string OpenId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public DateTimeOffset BindTime { get; set; }
    }
}
