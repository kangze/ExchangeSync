using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Common.Tools;
using ExchangeSync.Model.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Model.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DbContextOptions<ServiceDbContext> _dbOption;
        private readonly IMapper _mapper;

        public EmployeeService(DbContextOptions<ServiceDbContext> dbOption)
        {
            _dbOption = dbOption;

        }


        public async Task<List<EmployeeDto>> FindByUserNamesAsync(string[] userNames)
        {
            Check.NotNull(userNames);

            var emptyEmployees = new List<EmployeeDto>();
            if (userNames.Length == 0)
                return emptyEmployees;
            var names = this.ProcessUserNameIfContainsAt(userNames);
            using (var db = new ServiceDbContext(this._dbOption))
            {
                var employees = await db.Employees.Where(u => names.Contains(u.UserName)).ToListAsync();
                if (employees.Count == 0)
                    return emptyEmployees;
                var numbers = employees.Select(u => u.Number).ToList();
                var auths = await db.EmployeeAuths.Where(u => numbers.Contains(u.Number)).ToListAsync();

                foreach (var item in employees)
                {
                    var auth = auths.FirstOrDefault(u => u.Number == item.Number);
                    var employee = this._mapper.Map<EmployeeDto>(item);
                    if (auth != null) employee.Password = auth.Password.DecodeBase64();
                    employee.Account = employee.UserName + "@scrbg.com";
                    emptyEmployees.Add(employee);
                }
            }
            return emptyEmployees;
        }

        public async Task<EmployeeDto> FindByUserNameAsync(string userName)
        {
            var employees = await this.FindByUserNamesAsync(new string[] { userName });
            if (employees.Count == 0)
                return null;
            return employees[0];
        }

        private List<string> ProcessUserNameIfContainsAt(string[] userNames)
        {
            var names = new List<string>();
            foreach (var username in userNames)
            {
                if (string.IsNullOrEmpty(username) || username[0] == '@')
                    continue;
                if (username.Contains("@"))
                {
                    var splited = username.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splited.Length == 0)
                        continue;
                    names.Add(splited[0]);
                }
                else
                {
                    names.Add(username);
                }
            }
            return names;
        }
    }
}
