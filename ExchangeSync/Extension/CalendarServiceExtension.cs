using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Models;
using ExchangeSync.Services;

namespace ExchangeSync.Extension
{
    public static class CalendarServiceExtension
    {
        public static List<object> GroupedCalendarAppointments(this ICalendarService service, List<AppointMentDto> list)
        {
            var viewModles = list.Select(u => new AppointMentViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = u.Subject,
                Year = u.Start.Year,
                Month = u.Start.Month,
                Day = u.Start.Day,
                Week = Convert.ToInt32(u.Start.DayOfWeek),
            });
            var groups = viewModles.GroupBy(u => new { year = u.Year,month = u.Month })
                .OrderBy(u => u.Key.year).ThenBy(u => u.Key.month).ToList();
            var result = new List<object>();
            foreach (var item in groups)
            {
                var appoints = new { key = item.Key, data = item.ToList() };
                result.Add(appoints);
            }
            return result;
        }
    }
}
