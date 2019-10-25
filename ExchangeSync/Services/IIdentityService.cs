using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IIdentityService
    {
        /// <summary>
        /// 获取客户端AccessToken
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        Task<string> GetClientCredentialAccessTokenAsync(string scope, string clientId, string clientSecret);

        Task<string> GetUserAccessTokenAsync(string userName, string userPassword);

        Task<IEnumerable<Claim>> GetUserInfoAsync(string accessToken);
    }
}
