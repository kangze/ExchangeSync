using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class EmployeeEmail
    {
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public string OpenId { get; set; }

        public string Password { get; set; }
    }
}
