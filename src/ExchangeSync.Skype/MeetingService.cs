using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeSync.Skype
{
    public class MeetingService : IMeetingService
    {
        private readonly SkypeBootstraper _bootstraper;
        private readonly HttpClient _httpClient;

        public MeetingService(SkypeBootstraper bootstraper, HttpClient httpClient)
        {
            _bootstraper = bootstraper;
            _httpClient = httpClient;
        }

        public async Task<CreateMeetingResult> CreateOnlineMeetingAsync(string subject, string description, string account, string password)
        {
            await this._bootstraper.StartAsync(account, password);

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                subject = subject,
                description = description,
            }), Encoding.UTF8, "application/json");
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._bootstraper.OAuthToken.AccessToken);
            var uri = new Uri(this._bootstraper.Host + this._bootstraper.SkypeResourceLink.MyOnlineMeetings);
            var reponse = await this._httpClient.PostAsync(uri, content);
            var result = await reponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(result);
            return new CreateMeetingResult()
            {
                OnlineMeetingId = jObject["onlineMeetingId"].ToString(),
                Subject = jObject["subject"].ToString(),
                Description = jObject["description"].ToString(),
                JoinUrl = jObject["joinUrl"].ToString(),
            };

        }
    }
}
