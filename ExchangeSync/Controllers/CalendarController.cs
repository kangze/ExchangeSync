using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var viewModles = list.Select(u => new AppointMentViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = u.Subject,
                Year = u.Start.Year,
                Month = u.Start.Month,
                Day = u.Start.Day,
                Week = Convert.ToInt32(u.Start.DayOfWeek),
            });
            var groups = viewModles.GroupBy(u => new { u.Year, u.Month })
                .OrderBy(u => u.Key.Year).ThenBy(u => u.Key.Month).ToList();
            var result = new List<object>();
            foreach (var item in groups)
            {
                var appoints = new { key = item.Key, data = item.ToList() };
                result.Add(appoints);
            }
            return Json(result);
        }

        public async Task<IActionResult> CreateAppointMent(AppointMenInput input)
        {
            if (input == null)
                return Json(new { success = false });
            if (input.AddToSkype)
            {
                var skypeResult = await this._meetingService.CreateOnlineMeetingAsync(input.Title, input.Body);
                input.Body += skypeResult.JoinUrl;
            }
            await this._calendarService.CreateAppointMentAsync(input);
            return Json(true);
        }
    }
}
