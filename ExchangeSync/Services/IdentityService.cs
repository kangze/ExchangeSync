using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<string> GetUserAccessTokenAsync(string userName, string userPassword, string scope)
        {
            var disco = await GetDiscoveryDocumentResponseAsync();
            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                UserName = userName,
                Password = userPassword,
                ClientId = this._option.ClientId,
                ClientSecret = this._option.ClientSecret,
                Scope = scope,
            });
            return tokenResponse.Raw;
        }

        public async Task<string> GetUserInfoAsync(string accessToken)
        {
            var disco = await GetDiscoveryDocumentResponseAsync();
            var userInfoResponse = await _httpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken,
            });
            if (userInfoResponse.IsError) throw new Exception(userInfoResponse.Error);
            return userInfoResponse.Raw;
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentResponseAsync()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _option.IssuerUri,
                Policy =
                {
                    RequireHttps = _option.RequireHttps
                }
            });
            if (disco.IsError) throw new Exception(disco.Error);
            return disco;
        }
    }
}
