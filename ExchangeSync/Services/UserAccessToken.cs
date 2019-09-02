using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ExchangeSync.Services
{
    public class UserAccessToken
    {
        public bool Success { get; set; }

        public string AccessToken { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
