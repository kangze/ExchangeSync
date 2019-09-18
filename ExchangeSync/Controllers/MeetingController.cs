using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class MeetingController : Controller
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            this._meetingService = meetingService;
        }

        public async Task<IActionResult> Create(string subject, string description)
        {
            var result = await this._meetingService.CreateOnlineMeetingAsync(subject, description);
            return Json(result);
        }
    }
}
