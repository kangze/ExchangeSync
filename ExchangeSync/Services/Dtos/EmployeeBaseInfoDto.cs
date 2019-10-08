using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services.Dtos
{
    public class EmployeeBaseInfoDto
    {
        public Guid Id { get; set; }

        public string OpenId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string EmailAddress { get; set; }

        public string EmailPassword { get; set; }
    }
}
