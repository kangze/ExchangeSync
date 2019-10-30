using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.Dtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? UserId { get; set; }

        public string OpenId { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        public string IdCardNo { get; set; }

        public string Mobile { get; set; }

        public int Gender { get; set; }

        public string Avatar { get; set; }

        public Guid PrimaryDepartmentId { get; set; }

        public Guid PrimaryPositionId { get; set; }

    }
}
