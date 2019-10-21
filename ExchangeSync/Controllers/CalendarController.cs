using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Extension;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using ExchangeSync.Services;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IMeetingService _meetingService;

        public CalendarController(ICalendarService calendarService, IMeetingService meetingService)
        {
            _calendarService = calendarService;
            _meetingService = meetingService;
        }

        /// <summary>
        /// 获取我的会议
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MyAppointMents()
        {
            var list = await this._calendarService.GetMyAppointmentsAsync();
            var result = this._calendarService.GroupedCalendarAppointments(list);
            return Json(result);
        }

        public async Task<IActionResult> CreateAppointMent([FromBody]AppointMenInput input)
        {
            if (input == null)
                return Json(new { success = false });
            if (input.AddToSkype)
            {
                //var skypeResult = await this._meetingService.CreateOnlineMeetingAsync(input.Title, input.Body);
                //var joinhttp = skypeResult.JoinUrl;
                var joinhttp = "https://meet.scrbg.com/v-ms-kz/HDED4XL3";
                var joinUrl = "<a target=\"blank\" href =\"" + joinhttp + "\">点击参加Skype会议</a>";
                input.Body += joinUrl;
            }
            await this._calendarService.CreateAppointMentAsync(input);
            return Json(new { success = true });
        }
    }
}
