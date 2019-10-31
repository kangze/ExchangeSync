using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model;
using ExchangeSync.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;

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
            //var employeeEmail = 
            //if (employeeEmail == null || employeeEmail.Employee == null)
            throw new Exception("没有找到该用户相关的信息!");
            //return new EmployeeBaseInfoDto()
            //{
            //    Id = employeeEmail.EmployeeId,
            //    Name = employeeEmail.Employee.Name,
            //    Number = employeeEmail.Employee.Number,
            //    EmailAddress = employeeEmail.Employee.UserName + "@scrbg.com",
            //    OpenId = employeeEmail.OpenId,
            //    UserId = employeeEmail.Employee.UserId.HasValue ? employeeEmail.Employee.UserId.ToString() : null,
            //    EmailPassword = employeeEmail.Password
            //};
        }

        public async Task<List<EmployeeBaseInfoDto>> SearchEmployeeBaseInfoByKeyword(string keyword)
        {
            var ls = new List<EmployeeBaseInfoDto>();
            if (string.IsNullOrEmpty(keyword))
                return ls;
            var employees = await this._db.Employees
                //.Include(u => u.EmployeeEmail)
                .Where(u => (u.Name.Contains(keyword) || u.Number.Contains(keyword) || u.UserName.Contains(keyword)) && !string.IsNullOrEmpty(u.UserName))
                .OrderBy(u => u.Number)
                .Take(6)
                .ToListAsync();
            foreach (var employee in employees)
            {
                ls.Add(new EmployeeBaseInfoDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Number = employee.Number,
                    EmailAddress = employee.UserName + "@scrbg.com",
                    //OpenId = employee.EmployeeEmail == null ? null : employee.EmployeeEmail.OpenId,
                    //UserId = employee.UserId.Value.ToString(),
                    //EmailPassword = employee.EmployeeEmail == null ? null : employee.EmployeeEmail.Password
                });
            }
            return ls;
        }
    }
}
