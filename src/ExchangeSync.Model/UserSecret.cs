using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model
{
    public class UserSecret
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 用户邮箱账号
        /// </summary>
        public string UserAccount { get; set; }

        public string Number { get; set; }

        public string IdCardNo { get; set; }

        public string Password { get; set; }
    }
}
