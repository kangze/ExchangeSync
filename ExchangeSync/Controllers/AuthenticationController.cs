using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using ExchangeSync.Models;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ExchangeSync.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IIdentityInfoConverter _identityInfoConverter;
        private readonly IBindService _bindService;

        public AuthenticationController(IIdentityService identityService, IIdentityInfoConverter identityInfoConverter, IBindService bindService)
        {
            _identityService = identityService;
            _identityInfoConverter = identityInfoConverter;
            _bindService = bindService;
        }

        public IActionResult Connect()
        {
            //TODO:将会添加一个页面进行登录验证
            return View();
        }

        /// <summary>
        /// Login via SSO and connect Wechat
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> Connect(string userName, string password, string openId)
        //{
        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        //    {
        //        var errorMsg = ConnectStatusCode.AccountOrPasswordEmpty.ToDescription();
        //        return View(errorMsg);
        //    }

        //    var tokenRaw = await this._identityService.GetUserAccessTokenAsync(userName, userName, "openid profile profile.ext");
        //    var accessTokenResponse = this._identityInfoConverter.DeserializeUserTokenFromRaw(tokenRaw);
        //    if (!accessTokenResponse.Success) return View(accessTokenResponse.Message);

        //    var userInfoRaw = await this._identityService.GetUserInfoAsync(accessTokenResponse.AccessToken);
        //    var userInfo = this._identityInfoConverter.DeserializeUserInfoFromRaw(userInfoRaw);

        //    //begin bind with wechat
        //    await this._bindService.ConnectWithWeChatAsync(userInfo, password, openId);
        //    return Redirect("~/");
        //}
    }
}
