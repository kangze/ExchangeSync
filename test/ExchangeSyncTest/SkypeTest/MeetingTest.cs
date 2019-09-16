using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Skype;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeSyncTest.SkypeTest
{
    [TestClass]
    public class MeetingTest
    {
        [TestMethod]
        public async Task GetAuthenticateAddressAsync_Test()
        {
            var service = new MeetingService(new HttpClient(), new SkypeOption());
            var s = await service.GetAuthenticateAddressAsync(
                @"https://sfbpool.scrbg.com/Autodiscover/AutodiscoverService.svc/root/oauth/user?originalDomain=scrbg.com");
        }
    }
}
