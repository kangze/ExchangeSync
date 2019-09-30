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

        public HomeController(IServerRenderService serverRenderService)
        {
            _serverRenderService = serverRenderService;
        }

        public IActionResult Index()
        {
            var html = this._serverRenderService.Render();
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
