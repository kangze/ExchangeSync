using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model;
using ExchangeSync.Services.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Services
{
    public class EnterpriseContactService : IEnterpriseContactService
    {
        private readonly ServiceDbContext _db;

        public EnterpriseContactService(ServiceDbContext db)
        {
            _db = db;
        }

        public async Task<EmployeeBaseInfoDto> GetEmployeeBaseInfoAsync(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                throw new ArgumentNullException(nameof(openId));
            openId = openId.ToLower();
            var employeeEmail = await this._db.EmployeeEmails
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.OpenId == openId);
            if (employeeEmail == null || employeeEmail.Employee == null)
                throw new Exception("没有找到该用户相关的信息!");
            return new EmployeeBaseInfoDto()
            {
                Id = employeeEmail.EmployeeId,
                Name = employeeEmail.Employee.Name,
                Number = employeeEmail.Employee.Number,
                EmailAddress = employeeEmail.Employee.UserName + "@scrbg.com",
                OpenId = employeeEmail.OpenId,
                UserId = employeeEmail.Employee.UserId.HasValue ? employeeEmail.Employee.UserId.ToString() : null,
                EmailPassword = employeeEmail.Password
            };
        }
    }
}
