using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using Microsoft.AspNetCore.Mvc;
using ExchangeSync.Models;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Controllers
{
    public class HomeController : ExchangeControllerBase
    {
        private readonly IServerRenderService _serverRenderService;
        private readonly IMailService _mailService;
        private readonly ICalendarService _calendarService;
        private readonly ServiceDbContext _db;
        private readonly IIdentityService _identityService;

        public HomeController(IServerRenderService serverRenderService, IMailService mailService, ICalendarService calendarService, ServiceDbContext db, IIdentityService identityService)
        {
            this._serverRenderService = serverRenderService;
            this._mailService = mailService;
            _calendarService = calendarService;
            _db = db;
            _identityService = identityService;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                var accessToken = await this._identityService.GetUserAccessTokenAsync("scbzzx", "a123456");
                var claims = await this._identityService.GetUserInfoAsync(accessToken);
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim("access_token", accessToken));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect("~/");
            }
            var path = Request.Path.ToString();
            //get userName
            var number = this.GetNumber();
            var userName = this.GetUserName();
            var name = this.GetName();
            var user = new { userName = userName, name = name };
            var auth = await this._db.EmployeeAuths.FirstOrDefaultAsync(u => u.Number == number);
            if (auth == null) return Content("身份验证失败");
            var employee = await this._db.Employees.FirstOrDefaultAsync(u => u.Number == number);
            if (employee == null) return Content("身份验证失败");
            object data = null;
            if (path == "" || path == "/")
                data = await this._mailService.GetIndexMailAsync(number);
            else if (path.Contains("sended"))
                data = await this._mailService.GetSendedMailAsync(number);
            else if (path.Contains("draft"))
                data = await this._mailService.GetDraftMailAsync(number);
            else if (path.Contains("detail"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(number, mailId);
            }
            else if (path.Contains("calendar"))
            {
                data = await this._calendarService.GetMyAppointmentsAsync(employee.UserName + "@scrbg.com", auth.Password.DecodeBase64());
                //data = this._calendarService.GroupedCalendarAppointments(data as List<AppointMentDto>);
                data = (data as List<AppointMentDto>).Where(u => u.Start.Year == DateTime.Now.Year && u.Start.Month == DateTime.Now.Month && u.Start.Day == DateTime.Now.Day).ToList();
            }
            else if (path.Contains("reply"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(number, mailId);
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
