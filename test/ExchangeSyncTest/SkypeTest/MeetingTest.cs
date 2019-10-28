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
        public async Task GetSkypeResourceUrlAsync_Tetst()
        {
            var service0 = new SkypeBootstraper(new HttpClient(), new SkypeOption()
            {
                DiscoverServer = "http://lyncdiscoverinternal.scrbg.com/"
            });
            var service = new MeetingService(service0, new HttpClient());
           // await service.CreateOnlineMeetingAsync("测试", "描述");
        }
    }
}
