﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeSync.Controllers
{
    public class UserController : Controller
    {
        private readonly IEnterpriseContactService _enterpriseContactService;

        public UserController(IEnterpriseContactService enterpriseContactService)
        {
            _enterpriseContactService = enterpriseContactService;
        }

        public async Task<IActionResult> GetUser(string keyword)
        {
            var ls = new List<object>();
            try
            {
                var employees = await this._enterpriseContactService.SearchEmployeeBaseInfoByKeyword(keyword);
                foreach (var infoDto in employees)
                    ls.Add(new { key = infoDto.EmailAddress, name = infoDto.Name + "_" + infoDto.Number });
            }
            catch (Exception e)
            {

            }
            ls.Add(new { key = keyword, name = keyword });

            return Json(ls);
        }
    }
}
