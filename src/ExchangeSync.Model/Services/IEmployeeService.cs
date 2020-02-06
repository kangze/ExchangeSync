using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model.Dtos;

namespace ExchangeSync.Model.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// 查找用户资料
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns></returns>
        Task<List<EmployeeDto>> FindByUserNamesAsync(string[] userNames);

        Task<EmployeeDto> FindByUserNameAsync(string userName);

        Task<EmployeeDto> FindByUserNumberAsync(string userNumber);
    }
}
