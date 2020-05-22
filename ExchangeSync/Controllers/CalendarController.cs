using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using ExchangeSync.Model.Services;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using ExchangeSync.Services;
using ExchangeSync.Services.Dtos;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Controllers
{
    public class CalendarController : ExchangeControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly IMeetingService _meetingService;
        private readonly IOaSystemOperationService _oaSystemOprationService;
        private readonly IEmployeeService _employeeService;

        public CalendarController(
            ICalendarService calendarService,
            IMeetingService meetingService,
            IOaSystemOperationService oaSystemOprationService,
            IEmployeeService employeeService
            )
        {
            _employeeService = employeeService;
            _calendarService = calendarService;
            _meetingService = meetingService;
            _oaSystemOprationService = oaSystemOprationService;
        }

        /// <summary>
        /// 获取我的会议
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MyAppointMents(int year, int month, int day)
        {
            var userName = this.GetUserName();
            var employee = await this._employeeService.FindByUserNameAsync(userName);
            if (employee == null)
                return BadRequest();
            var list = await this._calendarService.GetMyAppointmentsAsync(employee.Account, employee.Password);
            var result = list.Where(u => u.Start.Year == year && u.Start.Month == month && u.Start.Day == day).ToList();
            foreach (var item in result)
            {
                if (item.Attendees != null && item.Attendees.Count == 1)
                {
                    var attends = item.Attendees[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    item.Attendees = attends;
                }
            }
            return Json(result);
        }

        public async Task<IActionResult> DeleteMeeting([FromForm] DeleteMeeetingInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Number) || string.IsNullOrWhiteSpace(input.Id))
            {
                return Json(new
                {
                    success = false,
                    error = "参数不能为空",
                    debug = input
                });
            }

            var employee = await this._employeeService.FindByUserNumberAsync(input.Number);
            if (employee == null)
            {
                return Json(new
                {
                    success = false,
                    error = "没有找到用户",
                    debug = input
                });
            }

            var result = await this._calendarService.DeleteMeetingAsync(employee.Account, employee.Password, input.Id);
            return Json(
                new
                {
                    success = true,
                    error = "",
                    debug = input
                }
            );
        }

        /// <summary>
        /// 给协同办公的创建会议的接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Createmeeting([FromForm]CreateMeetingInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Creator))
                return Json(new
                {
                    success = false,
                    error = "参数或者会议发起人不能为空",
                    onlinelink = "",
                    debug = input
                });
            var employee = await this._employeeService.FindByUserNumberAsync(input.Creator.Trim());
            if (employee == null)
                return Json(new
                {
                    id = "",
                    success = false,
                    error = "没有找到会议发起人",
                    onlinelink = "",
                    debug = input
                });
            try
            {


                var online = "";
                if (input.OnLine)
                {
                    var skypeResult = await this._meetingService.CreateOnlineMeetingAsync(input.Title, input.Body,
                        employee.Account, employee.Password);
                    var joinhttp = skypeResult.JoinUrl;
                    online = joinhttp;
                    var joinUrl = "<a target=\"blank\" href =\"" + joinhttp + "\">点击参加Skype会议</a>";
                    input.Body += joinUrl;
                }

                var attendEmployees = await this._employeeService.FindByUserNumbersAsync(input.Attendees.ToArray());

                var id = await this._calendarService.CreateAppointMentAsync(new AppointMenInput()
                {
                    AddToSkype = input.OnLine,
                    Attachment = new List<IFormFile>(),
                    Attachments = new List<AttachmentInput>(),
                    Attendees = attendEmployees.Select(u => u.Account).ToList(),
                    Body = input.Body,
                    Title = input.Title,
                    Start = DateTime.Parse(input.Start).Date.ToString("yyyy-MM-dd"),
                    StartTime = DateTime.Parse(input.Start).Date.ToString("HH:mm:ss"),
                    End = DateTime.Parse(input.End).Date.ToString("yyyy-MM-dd"),
                    EndTime = DateTime.Parse(input.End).Date.ToString("HH:mm:ss"),
                    FullDay = false,
                    Location = input.Location,
                    Type = AppointMentType.Talk,
                }, employee.Account, employee.Password);

                return Json(new
                {
                    success = true,
                    id = id,
                    error = "",
                    onlinelink = online,
                    debug = input
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = true,
                    id = "",
                    error = e.ToString(),
                    onlinelink = "",
                    debug = input
                });
            }
        }


        /// <summary>
        /// 获取我的会议
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetForbidden(int year, int month, int day)
        {
            var userName = this.GetUserName();
            var employee = await this._employeeService.FindByUserNameAsync(userName);
            if (employee == null)
                return BadRequest();
            var result = await this._calendarService.GetMyAppointmentsAsync(employee.Account, employee.Password);
            var nowDate = new DateTime(year, month, day);
            var beforeDate = nowDate.AddMonths(-3);
            var afterDate = nowDate.AddMonths(3);

            //计算区间
            var forbiddenDays = new List<DateTime>();
            for (var start = beforeDate; start < afterDate; start = start.AddDays(1))
            {
                if (result.Count(u => u.Start.Year == start.Year && u.Start.Month == start.Month && u.Start.Day == start.Day) == 0)
                    forbiddenDays.Add(start);
            }
            forbiddenDays.RemoveAll(u => u.Year == DateTime.Now.Year && u.Month == DateTime.Now.Month && u.Day == DateTime.Now.Day);

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
            var userName = this.GetUserName();
            if (input.Attendees != null && input.Attendees.Count == 1)
            {
                var attends = input.Attendees[0].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                input.Attendees = attends;
            }
            var employee = await this._employeeService.FindByUserNameAsync(userName);
            if (employee == null)
                return BadRequest();
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
                var skypeResult = await this._meetingService.CreateOnlineMeetingAsync(input.Title, input.Body, employee.Account, employee.Password);
                var joinhttp = skypeResult.JoinUrl;
                var joinUrl = "<a target=\"blank\" href =\"" + joinhttp + "\">点击参加Skype会议</a>";
                input.Body += joinUrl;
            }
            try
            {
                var attendEmployees = await this._employeeService.FindByUserNamesAsync(input.Attendees.ToArray());
                //send to oa
                foreach (var item in attendEmployees)
                {
                    await this._oaSystemOprationService.CreateAppointmentAsync(new Services.Dtos.OAAppoinmentInputDto()
                    {
                        Keyword1 = input.Title,
                        First = "您有一条会议提醒",
                        MeetId = Guid.NewGuid().ToString(),
                        UserNum = item.Number,
                        Keyword2 = DateTime.Parse(input.Start + " " + input.StartTime).ToString("yyyy-MM-dd"),
                        Keyword3 = input.Location,
                        Remark = "",
                    }); ;
                }
            }
            catch (Exception e)
            {

            }

            await this._calendarService.CreateAppointMentAsync(input, employee.Account, employee.Password);
            return Json(new { success = true });
        }
    }
}
