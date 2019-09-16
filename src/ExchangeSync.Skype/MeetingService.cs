using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Skype.Internal;
using Newtonsoft.Json.Linq;

namespace ExchangeSync.Skype
{
    public class MeetingService : IMeetingService
    {
        private readonly HttpClient _httpClient;
        private readonly SkypeOption _option;

        public MeetingService(HttpClient httpClient, SkypeOption option)
        {
            _httpClient = httpClient;
            _option = option;
        }

        public async Task<SkypeDiscoverLink> CreateMeetingAsync(string skypeAccessToken)
        {
            var response = await this._httpClient.GetAsync(_option.DiscoverServer);
            if (!response.IsSuccessStatusCode)
                throw new SkypeDiscoverException(response.ReasonPhrase);
            var content = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(content);
            var discover = new SkypeDiscoverLink()
            {
                User = jObject["_links"]["user"]["href"].ToString(),
                Self = jObject["_links"]["self"]["href"].ToString(),
                XFrame = jObject["_links"]["xframe"]["href"].ToString(),
            };
            return discover;
        }

        public async Task<string> GetAuthenticateAddressAsync(string userAddress)
        {
            if (string.IsNullOrEmpty(userAddress))
                throw new ArgumentNullException(nameof(userAddress));
            var response = await this._httpClient.GetAsync(userAddress);
            if (response.StatusCode != HttpStatusCode.Unauthorized) return default;
            var headerValue = response.Headers.WwwAuthenticate.FirstOrDefault(u => u.Scheme == "MsRtcOAuth");
            if (headerValue == null) return default;

            var match = RegexHelper.UrlRegex.Match(headerValue.Parameter);
            if (!match.Success) return default;
            return match.Value;
        }
    }
}
