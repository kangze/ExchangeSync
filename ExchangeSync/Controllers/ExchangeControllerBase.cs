using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class ExchangeControllerBase : Controller
    {
        public string GetUserName()
        {
            var username_cliam = User.Claims.FirstOrDefault(u => u.Type == "preferred_username");
            var userName = username_cliam.Value;
            return userName;
        }

        public string GetName()
        {
            var name_claim = User.Claims.FirstOrDefault(u => u.Type == "name");
            var name = name_claim.Value;
            return name;
        }

        public string GetNumber()
        {
            var number_claim = User.Claims.FirstOrDefault(u => u.Type == "employee_number");
            var number = number_claim.Value;
            return number;
        }
    }
}
