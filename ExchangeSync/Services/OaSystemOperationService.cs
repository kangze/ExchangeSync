using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Extension;
using ExchangeSync.Services.Dtos;
using Newtonsoft.Json;

namespace ExchangeSync.Services
{
    public class OaSystemOperationService : IOaSystemOperationService
    {
        private readonly OaApiOption _option;
        private readonly HttpClient _httpClient;

        public OaSystemOperationService(OaApiOption option, HttpClient httpClient)
        {
            _option = option;
            _httpClient = httpClient;
        }

        public async Task<bool> CreateAppointmentAsync(OAAppoinmentInputDto input)
        {
            Check.NotNull(input, nameof(input));
            using (var content = new StringContent(JsonConvert.SerializeObject(input)))
            {
                var response = await this._httpClient.PostAsync(new Uri(this._option.CreateAppointment), content);
                if (!response.IsSuccessStatusCode)
                    return false;
                var resultContent = response.Content.ReadAsStringAsync();
                return Convert.ToBoolean(resultContent);
            }
        }

        public async Task<string> SendNewMailSync(string url, string mailId, string subject, string number,string first,string remark)
        {
            using (var httpClient = new HttpClient())
            {
                var body = new
                {
                    url = url,
                    meetid = "abc",
                    userNum = number,
                    first = first,
                    keyword1 = subject,
                    keyword2 = "",
                    keyword3 = "",
                    remark = remark,
                    templateIdS = "jkeuA_9dC5TU0kri2Heh8v3egIXz6gEDgDCSpwzdiXg",
                };
                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response =
                    await httpClient.PostAsync(
                        "http://newoa.scrbg.com/api/services/app/wxapi/SendMeetNoticeMsgForMeetByMeetIdStr", content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        
    }

    
}
