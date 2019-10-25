using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace ExchangeSync.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdSvrOption _option;
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient, IOptions<IdSvrOption> optionAccessor)
        {
            _option = optionAccessor.Value;
            _httpClient = httpClient;
        }

        public async Task<string> GetClientCredentialAccessTokenAsync(string scope, string clientId = null, string clientSecret = null)
        {
            var disco = await GetDiscoveryDocumentResponseAsync();

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = string.IsNullOrEmpty(clientId) ? this._option.ClientId : clientId,
                ClientSecret = string.IsNullOrEmpty(clientId) ? this._option.ClientSecret : clientSecret,
                Scope = scope
            });
            if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);

            return tokenResponse.AccessToken;
        }

        public async Task<string> GetUserAccessTokenAsync(string userName, string userPassword)
        {
            var disco = await GetDiscoveryDocumentResponseAsync();
            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                UserName = userName,
                Password = userPassword,
                ClientId = this._option.ClientId,
                ClientSecret = this._option.ClientSecret,
                Scope = string.Join(" ", this._option.Scopes)
            });
            return tokenResponse.AccessToken;
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(string accessToken)
        {
            var disco = await GetDiscoveryDocumentResponseAsync();
            var userInfoResponse = await _httpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken,
            });
            if (userInfoResponse.IsError) throw new Exception(userInfoResponse.Error);
            return userInfoResponse.Claims;
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentResponseAsync()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _option.IssuerUri,
                Policy =
                {
                    RequireHttps = _option.IssuerUri.Contains("https")
                }
            });
            if (disco.IsError) throw new Exception(disco.Error);
            return disco;
        }
    }
}
