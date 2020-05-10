using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using ExchangeSync.Model.Dtos;
using ExchangeSync.Model.Services;
using Microsoft.AspNetCore.Mvc;
using ExchangeSync.Models;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace ExchangeSync.Controllers
{
    public class HomeController : ExchangeControllerBase
    {
        private readonly IServerRenderService _serverRenderService;
        private readonly IMailService _mailService;
        private readonly ICalendarService _calendarService;
        private readonly ServiceDbContext _db;
        private readonly IIdentityService _identityService;
        private readonly IEmployeeService _employeeService;

        public HomeController(IServerRenderService serverRenderService, IMailService mailService, ICalendarService calendarService, ServiceDbContext db, IIdentityService identityService, IEmployeeService employeeService)
        {
            this._serverRenderService = serverRenderService;
            this._mailService = mailService;
            _calendarService = calendarService;
            _db = db;
            _identityService = identityService;
            _employeeService = employeeService;
        }

        //[Authorize]
        public async Task<IActionResult> Index1()
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
            var userName = this.GetUserName();
            var name = this.GetName();
            var employee = await this._employeeService.FindByUserNameAsync(userName);
            if (employee == null)
                return BadRequest();
            var wechat = false;
            var headersDictionary = Request.Headers;
            var agent = headersDictionary[HeaderNames.UserAgent].ToString();
            if (agent.ToLower().Contains("micromessenger"))
                wechat = true;
            var user = new { userName = userName, name = name, wechat = wechat };

            var data = await this.GetMailDataAsync(userName, employee);
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

        public string OpenS(string str)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(str);
                var decode = Encoding.GetEncoding("utf-8").GetString(bytes);
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   

                byte[] key = Encoding.Unicode.GetBytes("sclq"); //定义字节数组，用来存储密钥   

                byte[] data = Convert.FromBase64String(decode);//定义字节数组，用来存储要解密的字符串 

                MemoryStream MStream = new MemoryStream(); //实例化内存流对象     

                //使用内存流实例化解密流对象      
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);

                CStream.Write(data, 0, data.Length);      //向解密流中写入数据    

                CStream.FlushFinalBlock();               //释放解密流     

                return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串 
                //byte[] key = Encoding.Unicode.GetBytes("sclq");//密钥
                //byte[] data = Convert.FromBase64String(str);//待解密字符串

                //DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
                //MemoryStream MStream = new MemoryStream();//内存流对象

                ////用内存流实例化解密流对象
                //CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
                //CStream.Write(data, 0, data.Length);//向加密流中写入数据
                //CStream.FlushFinalBlock();//将数据压入基础流
                //byte[] temp = MStream.ToArray();//从内存流中获取字节序列
                //CStream.Close();//关闭加密流
                //MStream.Close();//关闭内存流

                //return Encoding.Unicode.GetString(temp);//返回解密后的字符串
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        public async Task<IActionResult> TT(string a, string p)
        {
            var accessToken22 = await this._identityService.GetUserAccessTokenAsync(a, p);
            var claims22 = await this._identityService.GetUserInfoAsync(accessToken22);
            var claimsIdentity22 = new ClaimsIdentity(claims22, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity22.AddClaim(new Claim("access_token", accessToken22));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity22));
            return Redirect("/");
        }

        //[HttpGet("mailIndex")]
        public async Task<IActionResult> Index(string number)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var willNumber = this.GetNumber();
                if (!string.IsNullOrWhiteSpace(number))
                {
                    var s_number = this.OpenS(number);
                    if (!string.IsNullOrWhiteSpace(s_number) && s_number != willNumber)
                    {
                        willNumber = s_number;
                        var employeeKK = await this._employeeService.FindByUserNumberAsync(willNumber);
                        var accessToken22 = await this._identityService.GetUserAccessTokenAsync(employeeKK.UserName, employeeKK.Password);
                        var claims22 = await this._identityService.GetUserInfoAsync(accessToken22);
                        var claimsIdentity22 = new ClaimsIdentity(claims22, CookieAuthenticationDefaults.AuthenticationScheme);
                        claimsIdentity22.AddClaim(new Claim("access_token", accessToken22));
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity22));
                    }
                }

                if (string.IsNullOrWhiteSpace(willNumber)) willNumber = this.GetNumber();


                var employee_logined = await this._employeeService.FindByUserNumberAsync(willNumber);
                var mailData1 = await this.GetMailDataAsync(employee_logined.UserName, employee_logined);
                var user1 = new { userName = employee_logined.UserName, name = employee_logined.Name, wechat = true };
                var html1 = this._serverRenderService.Render(Request.Path, mailData1, user1);
                return Content(html1, "text/html; charset=utf-8");
            }
            if (string.IsNullOrWhiteSpace(number)) return Content("加密内容不能为空!");
            var user_number = this.OpenS(number);
            if (string.IsNullOrWhiteSpace(user_number)) return Content("解密后内容不合法!");

            var employee = await this._employeeService.FindByUserNumberAsync(user_number);
            if (employee == null) return Content("程序内容未找到改用户:" + user_number);

            var accessToken = await this._identityService.GetUserAccessTokenAsync(employee.UserName, employee.Password);
            var claims = await this._identityService.GetUserInfoAsync(accessToken);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim("access_token", accessToken));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            var mailData = await this.GetMailDataAsync(employee.UserName, employee);
            var user = new { userName = employee.UserName, name = employee.Name, wechat = true };
            var html = this._serverRenderService.Render(Request.Path, mailData, user);
            return Content(html, "text/html; charset=utf-8");
        }

        private async Task<object> GetMailDataAsync(string userName, EmployeeDto employee)
        {
            object data = null;
            var path = Request.Path.ToString();
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
                var myAppointments = await this._calendarService.GetMyAppointmentsAsync(employee.Account, employee.Password);
                myAppointments = (myAppointments as List<AppointMentDto>).Where(u => u.Start.Year == DateTime.Now.Year && u.Start.Month == DateTime.Now.Month && u.Start.Day == DateTime.Now.Day).ToList();
                var nowDate = DateTime.Now;
                var beforeDate = nowDate.AddMonths(-3);
                var afterDate = nowDate.AddMonths(3);

                //计算区间
                var forbiddenDays = new List<DateTime>();
                for (var start = beforeDate; start < afterDate; start = start.AddDays(1))
                {
                    if (myAppointments.Count(u => u.Start.Year == start.Year && u.Start.Month == start.Month && u.Start.Day == start.Day) == 0)
                        forbiddenDays.Add(start);
                }
                forbiddenDays.RemoveAll(u => u.Year == DateTime.Now.Year && u.Month == DateTime.Now.Month && u.Day == DateTime.Now.Day);
                data = new { myAppointments = myAppointments, forbiddenDays = forbiddenDays };
            }
            else if (path.Contains("reply"))
            {
                var split = path.Split('/');
                if (split.Length != 3)
                    return Redirect("/");
                var mailId = split[2];
                data = await this._mailService.GetMailAsync(userName, mailId);
            }

            return data;
        }

        public IActionResult Max()
        {
            return Content(NotiMax.Current.ToString() + "---" + NotiMax.UserMax.ToString() + "----" + NotiMax.SubScriptionMax + "__----------" + NotiMax.MailManagerMax.Count);
        }

        public IActionResult Error1()
        {
            return Content(NotiMax.Error.Count + JsonConvert.SerializeObject(NotiMax.Error));
        }
    }
}
