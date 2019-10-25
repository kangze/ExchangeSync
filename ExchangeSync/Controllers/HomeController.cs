using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Extension;
using Microsoft.AspNetCore.Mvc;
using ExchangeSync.Models;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.NodeServices;

namespace ExchangeSync.Controllers
{
    public class HomeController : ExchangeControllerBase
    {
        private readonly IServerRenderService _serverRenderService;
        private readonly IMailService _mailService;
        private readonly ICalendarService _calendarService;

        public HomeController(IServerRenderService serverRenderService, IMailService mailService, ICalendarService calendarService)
        {
            this._serverRenderService = serverRenderService;
            this._mailService = mailService;
            _calendarService = calendarService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var path = Request.Path.ToString();
            //get userName
            var userName = this.GetUserName();
            var name = this.GetName();
            var user = new { userName = userName, name = name };
            object data = null;
            if (path == "" || path == "/")
                data = await this._mailService.GetIndexMailAsync(userName);
            else if (path.Contains("sended"))
                data = await this._mailService.GetSendedMailAsync(userName);
            else if (path.Contains("draft"))
                data = await this._mailService.GetDraftMailAsync(userName);
            else if (path.Contains("detail"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(userName, mailId);
            }
            else if (path.Contains("calendar"))
            {
                data = await this._calendarService.GetMyAppointmentsAsync();
                data = this._calendarService.GroupedCalendarAppointments(data as List<AppointMentDto>);
            }
            else if (path.Contains("reply"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(userName, mailId);
            }

            var html = this._serverRenderService.Render(Request.Path, data, user);
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
