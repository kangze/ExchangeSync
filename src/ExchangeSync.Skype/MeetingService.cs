﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public async Task CreateOnlineMeetingAsync(string subject, string description)
        {
            await this._bootstraper.StartAsync("v-ms-kz@scrbg.com", "tfs4418000");

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                subject = subject,
                description = description,
            }), Encoding.UTF8, "application/json");
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._bootstraper.OAuthToken.AccessToken);
            var uri = new Uri(this._bootstraper.Host + this._bootstraper.SkypeResourceLink.MyOnlineMeetings);
            var reponse = await this._httpClient.PostAsync(uri, content);
            var result = await reponse.Content.ReadAsStringAsync();
        }
    }
}
