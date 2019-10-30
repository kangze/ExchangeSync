using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
}
