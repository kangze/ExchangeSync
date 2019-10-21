using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Controllers
{
    public class ImagesController : Controller
    {
        public static Dictionary<string, string> Images { get; set; } = new Dictionary<string, string>();

        public IActionResult GetImage(string id)
        {
            if (Images.ContainsKey(id))
                return Content(Images[id]);
            return Content("");
        }
    }


}
