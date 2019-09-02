using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public class UserInfo
    {
        public Guid SsoId { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        public Guid MdmId { get; set; }

        public string IdCardNo { get; set; }


    }
}
