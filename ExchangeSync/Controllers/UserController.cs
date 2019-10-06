using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeSync.Controllers
{
    public class UserController : Controller
    {
        public IActionResult GetUser(string keyword)
        {
            var list = new List<object>()
            {
                new {key="v-ms-kz@scrbg.com",name="康泽(015152)"},
                new {key="v-ms-kkkk@scrbg.com",name="康泽(0432432)"},
            };
            return Json(list);
        }
    }
}
