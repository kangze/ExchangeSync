using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class ValidateController : Controller
    {
        [HttpGet]
        [HttpPost]
        [Route("/Validate.html")]
        public IActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            var req = this.Request;
            var s = (new StreamReader(this.Request.Body)).ReadToEnd();
            return Content(echostr);
        }

        public async Task<IActionResult> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(
                "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx932d9b5eb387da28&secret=57e9a033d2fc8c0b767e191cfa2ff930");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json);

        }
    }
}
