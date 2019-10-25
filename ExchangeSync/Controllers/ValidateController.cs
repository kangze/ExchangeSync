using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class ValidateController : Controller
    {
        private readonly IIdentityService _identityService;

        public ValidateController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

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

        /// <summary>
        /// 用于单点登录的验证回调
        /// </summary>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthticationCallback(string a, string p)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(p))
                return BadRequest("空的用户账户信息!");
            var accessToken = await this._identityService.GetUserAccessTokenAsync(a.Trim(), p.Trim());
            var claims = await this._identityService.GetUserInfoAsync(accessToken);
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            claimsIdentity.AddClaim(new Claim("access_token", accessToken));
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
            return Redirect("~/");
        }
    }
}
