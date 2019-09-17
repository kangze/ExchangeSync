using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public class OAuthToken
    {
        public string AccessToken { get; set; }

        public int ExpireIn { get; set; }

        public string IdentityScope { get; set; }

        public string TokenType { get; set; }
    }
}
