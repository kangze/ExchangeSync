using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExchangeSync.Services
{
    public class IdentityInfoConverter : IIdentityInfoConverter
    {
        public UserAccessToken DeserializeUserTokenFromRaw(string raw)
        {
            var jObject = JObject.Parse(raw);
            if (jObject.TryGetValue("access_token", out var jToken))
                return new UserAccessToken()
                {
                    Success = true,
                    AccessToken = jToken.ToString()
                };
            return new UserAccessToken()
            {
                StatusCode = Convert.ToInt32(jObject["StatusCode"]),
                Message = jObject["StatusCodeDescription"].ToString(),
            };
        }

        public UserInfo DeserializeUserInfoFromRaw(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                throw new ArgumentNullException(nameof(raw));
            var jObject = JObject.Parse(raw);
            return new UserInfo()
            {
                //SsoId = Guid.Parse(jObject["sub"].ToString()),
                Name = jObject["name"].ToString(),
                UserName = jObject["preferred_username"].ToString(),
                Number = jObject["employee_number"].ToString(),
                IdCardNo = jObject["idcard_number"].ToString(),
                //MdmId = Guid.Parse(jObject["employee_mdmid"].ToString()),
            };
        }
    }
}
