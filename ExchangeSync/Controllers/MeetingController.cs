using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model;
using ExchangeSync.Model.Services;
using ExchangeSync.Skype;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Controllers
{
    public class MeetingController : ExchangeControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IEmployeeService _employeeService;

        public MeetingController(IMeetingService meetingService, IEmployeeService employeeService)
        {
            this._meetingService = meetingService;
            _employeeService = employeeService;
        }

        [Authorize]
        public async Task<IActionResult> Create(string subject, string description)
        {
            var userName = this.GetUserName();
            var employee = await this._employeeService.FindByUserNameAsync(userName);
            var result = await this._meetingService.CreateOnlineMeetingAsync(subject, description, employee.Account, employee.Password);
            return Json(result);
        }
    }
}
