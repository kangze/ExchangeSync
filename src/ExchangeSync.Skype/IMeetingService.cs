using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public interface IMeetingService
    {
        /// <summary>
        /// create a online meeting by skyep UCWA
        /// </summary>
        /// <param name="skypeAccessToken">skype服务的AccessToken</param>
        /// <returns></returns>
        Task<SkypeDiscoverLink> CreateMeetingAsync(string skypeAccessToken);


        Task<string> GetAuthenticateAddressAsync(string userAddress);
    }
}
