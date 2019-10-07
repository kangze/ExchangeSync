using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExchangeSync.Models;
using ExchangeSync.Services;
using Microsoft.AspNetCore.NodeServices;

namespace ExchangeSync.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServerRenderService _serverRenderService;
        private readonly IMailService _mailService;

        public HomeController(IServerRenderService serverRenderService, IMailService mailService)
        {
            this._serverRenderService = serverRenderService;
            this._mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            //这里统一分析所有的url
            //1./detail/2222
            //2./index  服务器端渲染

            //new
            //{
            //    title = "服务器标题",
            //    sender = "server",
            //    date = "2017-8-8",
            //    content = "服务器内容",
            //}
            var path = Request.Path.ToString();
            object data = null;
            if (path == "" || path == "/")
                data = await this._mailService.GetIndexMailAsync("");
            else if (path.Contains("sended"))
                data = await this._mailService.GetSendedMailAsync("");
            else if (path.Contains("draft"))
                data = await this._mailService.GetDraftMailAsync("");
            else if (path.Contains("detail"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(mailId);
            }
            else if (path.Contains("reply"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(mailId);
            }

            var html = this._serverRenderService.Render(Request.Path, data);
            return Content(html, "text/html; charset=utf-8");
        }

        public async Task<IActionResult> Privacy(string code)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(
                string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx932d9b5eb387da28&secret=57e9a033d2fc8c0b767e191cfa2ff930&code={0}&grant_type=authorization_code", code));
            var json = await response.Content.ReadAsStringAsync();
            //return Content(json);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
