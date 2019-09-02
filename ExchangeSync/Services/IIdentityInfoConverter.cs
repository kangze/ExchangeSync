using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IIdentityInfoConverter
    {
        UserAccessToken DeserializeUserTokenFromRaw(string raw);

        UserInfo DeserializeUserInfoFromRaw(string raw);
    }
}
