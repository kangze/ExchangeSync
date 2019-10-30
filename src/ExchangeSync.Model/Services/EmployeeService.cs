using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Common.Tools;
using ExchangeSync.Model.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Model.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DbContextOptions<ServiceDbContext> _dbOption;

        public EmployeeService(DbContextOptions<ServiceDbContext> dbOption)
        {
            _dbOption = dbOption;
        }


        public Task<List<EmployeeDto>> FindByUserName(string[] userNames)
        {
            Check
        }
    }
}
