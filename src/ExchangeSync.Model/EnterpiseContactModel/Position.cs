using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class Position
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public Guid DepartmentId { get; set; }

        public DataSourceType DataSourceType { get; set; }

        public virtual Department Department { get; set; }
    }
}
