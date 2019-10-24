using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services.Dtos
{
    public class SsoOption
    {
        public string Address { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string[] Scopes { get; set; }
    }
}
