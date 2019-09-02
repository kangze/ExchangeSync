using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model
{
    public class UserConnect
    {
        /// <summary>
        /// MdmId,sync from MDM
        /// </summary>
        public Guid Id { get; set; }

        public Guid SsoId { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        [StringLength(64)]
        public string UserName { get; set; }

        [StringLength(32)]
        public string Number { get; set; }

        [StringLength(64)]
        public string IdCard { get; set; }

        [StringLength(256)]
        public string HashPassword { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
