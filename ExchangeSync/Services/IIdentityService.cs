using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IIdentityService
    {
        /// <summary>
        /// 获取客户端AccessToken
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        Task<string> GetClientCredentialAccessTokenAsync(string clientId, string clientSecret, string scope);

        Task<string> GetUserAccessTokenAsync(string userName, string userPassword, string scope);

        Task<string> GetUserInfoAsync(string accessToken);
    }
}
