using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class Employee
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public Guid? UserId { get; set; }

        [StringLength(128)]
        public string OpenId { get; set; }

        public string Password { get; set; }

        [StringLength(128)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Number { get; set; }

        [Required]
        [StringLength(128)]
        public string IdCardNo { get; set; }


        [StringLength(128)]
        public string Mobile { get; set; }

        public int Gender { get; set; }

        public string Avatar { get; set; }

        [Required]
        public Guid PrimaryDepartmentId { get; set; }

        [Required]
        public Guid PrimaryPositionId { get; set; }

        public DataSourceType DataSourceType { get; set; }

        public virtual List<EmployeePosition> Positions { get; set; }

    }
}
