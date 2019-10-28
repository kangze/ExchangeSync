using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using ExchangeSync.Services;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Controllers
{
    public class CalendarController : ExchangeControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly IMeetingService _meetingService;
        private readonly ServiceDbContext _db;

        public CalendarController(ICalendarService calendarService, IMeetingService meetingService, ServiceDbContext db)
        {
            _calendarService = calendarService;
            _meetingService = meetingService;
            _db = db;
        }

        private async Task<(string, string)> GetAccountAndPassword()
        {
            var number = this.GetNumber();
            var auth = await this._db.EmployeeAuths.FirstOrDefaultAsync(u => u.Number == number);
            if (auth == null) return (null, null);
            var employee = await this._db.Employees.FirstOrDefaultAsync(u => u.Number == number);
            if (employee == null) return (null, null);
            return (employee.UserName + "@scrbg.com", auth.Password.DecodeBase64());
        }

        /// <summary>
        /// 获取我的会议
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MyAppointMents(int year, int month, int day)
        {
            var (userName, password) = await GetAccountAndPassword();
            var list = await this._calendarService.GetMyAppointmentsAsync(userName, password);
            var result = list.Where(u => u.Start.Year == year && u.Start.Month == month && u.Start.Day == day).ToList();
            return Json(result);
        }

        /// <summary>
        /// 获取我的会议
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetForbidden(int year, int month, int day)
        {
            var (userName, password) = await GetAccountAndPassword();
            var result = await this._calendarService.GetMyAppointmentsAsync(userName, password);
            var nowDate = new DateTime(year, month, day);
            var beforeDate = nowDate.AddMonths(-1);
            var afterDate = nowDate.AddMonths(1);

            //计算区间
            var forbiddenDays = new List<DateTime>();
            for (var start = beforeDate; start < afterDate; start = start.AddDays(1))
            {
                if (result.Count(u => u.Start.Year == start.Year && u.Start.Month == start.Month && u.Start.Day == start.Day) == 0)
                    forbiddenDays.Add(start);
            }
            return Json(forbiddenDays);
        }

        private void ConvertImage(AppointMenInput input)
        {
            //如果是图片的话,必须使用CID
            if (string.IsNullOrEmpty(input.Body))
                input.Body = "";
            if (input.Attachments == null)
                input.Attachments = new List<AttachmentInput>();
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            var matches = regImg.Matches(input.Body);
            if (matches.Count == 0)
                return;
            foreach (Match match in matches)
            {
                var imgSrc = match.Groups[1].Value;
                var imgBase64 = imgSrc.Split(',')[1];
                var imgBytes = Convert.FromBase64String(imgBase64);
                var attachmentId = Guid.NewGuid().ToString("N").ToLower() + ".jpg";
                input.Body = input.Body.Replace(imgSrc, "cid:" + attachmentId);

                input.Attachments.Add(new AttachmentInput()
                {
                    Id = attachmentId,
                    Name = attachmentId,
                    Bytes = imgBytes
                });
            }
        }

        [Authorize]
        public async Task<IActionResult> CreateAppointMent([FromForm]AppointMenInput input)
        {
            if (input == null)
                return Json(new { success = false });
            var (userName, password) = await GetAccountAndPassword();
            this.ConvertImage(input);
            if (input.Attachment != null)
            {
                foreach (var it in input.Attachment)
                {
                    var stream = it.OpenReadStream();
                    var bytes = new byte[it.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    var name = Guid.NewGuid().ToString("N").ToLower() + "-" + it.FileName;
                    input.Attachments.Add(new AttachmentInput()
                    {
                        Bytes = bytes,
                        Name = name,
                        Id = name,
                        IsPackage = true,
                    });
                }
            }
            if (input.AddToSkype)
            {
                var skypeResult = await this._meetingService.CreateOnlineMeetingAsync(input.Title, input.Body,userName,password);
                var joinhttp = skypeResult.JoinUrl;
                //var joinhttp = "https://meet.scrbg.com/v-ms-kz/HDED4XL3";
                var joinUrl = "<a target=\"blank\" href =\"" + joinhttp + "\">点击参加Skype会议</a>";
                input.Body += joinUrl;
            }
            await this._calendarService.CreateAppointMentAsync(input,userName,password);
            return Json(new { success = true });
        }
    }
}
