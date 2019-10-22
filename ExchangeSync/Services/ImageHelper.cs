using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public static class ImageHelper
    {
        public static Stream GetImageFromBase64(string base64Str)
        {
            var bytes = Convert.FromBase64String(base64Str);
            var memoryStream = new MemoryStream();
            memoryStream.WriteAsync(bytes, 0, bytes.Length);
            return memoryStream;
        }
    }
}
