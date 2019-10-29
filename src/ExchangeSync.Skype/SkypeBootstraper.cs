using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Skype.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeSync.Skype
{
    public class SkypeBootstraper //: IMeetingService
    {
        private readonly HttpClient _httpClient;
        private readonly SkypeOption _option;


        public SkypeDiscoverLink SkypeDiscoverLink { get; set; }
        public SkypeResourceDiscoverLink SkyResourceDiscoverLink { get; set; }
        public SkypeResourceLink SkypeResourceLink { get; set; }

        public string Host { get; set; }

        public OAuthToken OAuthToken { get; set; }

        public string AuthenticateAddress { get; set; }

        public SkypeBootstraper(HttpClient httpClient, SkypeOption option)
        {
            _httpClient = httpClient;
            _option = option;
        }

        public async Task GetSkypeFrameUrlAsync()
        {
            //var response = await this._httpClient.GetAsync(_option.DiscoverServer);
            //if (!response.IsSuccessStatusCode)
            //    throw new SkypeDiscoverException(response.ReasonPhrase);
            //var content = await response.Content.ReadAsStringAsync();

            //var jObject = JObject.Parse(content);
            //var discover = new SkypeDiscoverLink()
            //{
            //    User = jObject["_links"]["user"]["href"].ToString(),
            //    Self = jObject["_links"]["self"]["href"].ToString(),
            //    XFrame = jObject["_links"]["xframe"]["href"].ToString(),
            //};
            var discover = new SkypeDiscoverLink()
            {
                User = "https://sfbpool.scrbg.com/Autodiscover/AutodiscoverService.svc/root/oauth/user?originalDomain=scrbg.com",
                Self = "https://sfbpool.scrbg.com/Autodiscover/AutodiscoverService.svc/root?originalDomain=scrbg.com",
                XFrame = "https://sfbpool.scrbg.com/Autodiscover/XFrame/XFrame.html",
            };
            this.SkypeDiscoverLink = discover;
            var uri = new Uri(discover.User);
            this.Host = uri.Scheme + "://" + uri.Host;
        }

        public async Task GetAuthenticateAddressAsync(string userAddress)
        {
            if (string.IsNullOrEmpty(userAddress))
                throw new ArgumentNullException(nameof(userAddress));
            var response = await this._httpClient.GetAsync(userAddress);
            if (response.StatusCode != HttpStatusCode.Unauthorized) return;
            var headerValue = response.Headers.WwwAuthenticate.FirstOrDefault(u => u.Scheme == "MsRtcOAuth");
            if (headerValue == null) return;
            //href="https://sfbpool.scrbg.com/WebTicket/oauthtoken",grant_type="urn:microsoft.rtc:windows,urn:microsoft.rtc:windows,urn:microsoft.rtc:anonmeeting,password"
            //foreach (var item in headerValue.Parameter.Split(','))
            //{
            //    if (item.Contains("http"))
            //    {
            //        this.AuthenticateAddress = item.Replace("\"", "").Split('=')[1];
            //    }
            //}
            this.AuthenticateAddress = "https://sfbpool.scrbg.com/WebTicket/oauthtoken";
        }

        public async Task GetUserOAuthTokenAsync(string userName, string password)
        {
            var forms = new[]
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("userName",userName),
                new KeyValuePair<string, string>("password",password),
            };

            var response = await this._httpClient.PostAsync(new Uri("https://sfbpool.scrbg.com/WebTicket/oauthtoken"),
                new FormUrlEncodedContent(forms));
            var content = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(content);
            OAuthToken = new OAuthToken()
            {
                AccessToken = jObject["access_token"].ToString(),
                ExpireIn = Convert.ToInt32(jObject["expires_in"]),
                IdentityScope = jObject["ms_rtc_identityscope"].ToString(),
                TokenType = jObject["token_type"].ToString()
            };
        }

        public async Task GetSkypeResourceUrlAsync(string skypeAccessToken)
        {
            if (string.IsNullOrEmpty(skypeAccessToken))
                throw new ArgumentNullException(nameof(skypeAccessToken));
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", skypeAccessToken);
            var response = await this._httpClient.GetAsync(SkypeDiscoverLink.User);
            var content = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(content);
            this.SkyResourceDiscoverLink = new SkypeResourceDiscoverLink()
            {
                Applications = jObject["_links"]["applications"]["href"].ToString(),
                Self = jObject["_links"]["self"]["href"].ToString(),
                XFrame = jObject["_links"]["xframe"]["href"].ToString(),
            };
        }

        /// <summary>
        /// register application
        /// </summary>
        /// <param name="registion"></param>
        /// <returns></returns>
        public async Task RegisterApplicationsAsync(ApplicationRegistion registion)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registion), Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync(this.SkyResourceDiscoverLink.Applications, content);
            var resource = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(resource);
            this.SkypeResourceLink = new SkypeResourceLink()
            {
                MyOnlineMeetings = jObject["_embedded"]["onlineMeetings"]["_links"]["myOnlineMeetings"]["href"].ToString()
            };
        }

        /// <summary>
        /// Start a UCWA2.0 Application
        /// </summary>
        public async Task StartAsync(string userName, string password)
        {
            //step1:init discover urls
            await this.GetSkypeFrameUrlAsync();

            //step2:get 401 code
            await this.GetAuthenticateAddressAsync(this.SkypeDiscoverLink.User);

            //step3:get accessToken
            await this.GetUserOAuthTokenAsync(userName, password);

            //step4:get resource urls
            await this.GetSkypeResourceUrlAsync(this.OAuthToken.AccessToken);

            //step5:register applications
            await this.RegisterApplicationsAsync(new ApplicationRegistion()
            {
                EndPointId = Guid.NewGuid().ToString("N"),
                UserAgent = "SCRBG_APP"
            });

        }
    }
}
