﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeSyncTest.ServicesTest
{
    [TestClass]
    public class IdentityServiceTest
    {
        [TestMethod]
        public async Task GetUserAccessTokenAsync_Test()
        {
            var service = new IdentityService(new HttpClient(), new IdOptions(new IdSvrOption()
            {
                IssuerUri = "https://login.scrbg.com",
                RequireHttps = true,
                ClientId = "OM_BI_PORTAL_Web_001",
                ClientSecret = "OMBIPORTALWeb001",
                
            }));
            var userName = "003139";
            var password = "a123456";
            var access_token = await service.GetUserAccessTokenAsync(userName, password, "openid profile profile.ext");
            var userInfo = await service.GetUserInfoAsync(access_token);
            Assert.IsNotNull(access_token);
            Assert.IsNotNull(userInfo);
        }
    }

    public class IdOptions : IOptions<IdSvrOption>
    {
        public IdOptions(IdSvrOption option)
        {
            this.Value = option;
        }

        public IdSvrOption Value { get; }
    }
}