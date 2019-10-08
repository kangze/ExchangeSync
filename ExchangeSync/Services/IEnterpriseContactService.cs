using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Services.Dtos;

namespace ExchangeSync.Services
{
    public interface IEnterpriseContactService
    {
        Task<EmployeeBaseInfoDto> GetEmployeeBaseInfoAsync(string openId);

        /// <summary>
        /// 查找用户,关键字查找(name,username,number)
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<EmployeeBaseInfoDto>> SearchEmployeeBaseInfoByKeyword(string keyword);
    }
}
