using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Controllers
{
    public class MeetingController : ExchangeControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly ServiceDbContext _db;

        public MeetingController(IMeetingService meetingService, ServiceDbContext db)
        {
            this._meetingService = meetingService;
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Create(string subject, string description)
        {
            var number = this.GetNumber();
            var auth = await this._db.EmployeeAuths.FirstOrDefaultAsync(u => u.Number == number);
            if (auth == null) return null;
            var employee = await this._db.Employees.FirstOrDefaultAsync(u => u.Number == number);
            if (employee == null) return null;
            var result = await this._meetingService.CreateOnlineMeetingAsync(subject, description, employee.UserName + "@scrbg.com", auth.Password.DecodeBase64());
            return Json(result);
        }
    }
}
