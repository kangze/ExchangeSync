using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeSync.Model.EnterpiseContactModel
{
    public class EmployeePosition
    {
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public Guid PositionId { get; set; }

        public virtual Position Position { get; set; }

        public bool IsPrimary { get; set; }

        public DataSourceType DataSourceType { get; set; }
    }
}
