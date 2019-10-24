using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class EmployeeAuth
    {
        [Key]
        public string Number { get; set; }

        public string IdCardNo { get; set; }

        public string OpenId { get; set; }

        public string Password { get; set; }
    }
}
