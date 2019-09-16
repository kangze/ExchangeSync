using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeSync.Skype.Internal
{
    public static class RegexHelper
    {
        public static Regex UrlRegex = new Regex(@"^https?://(?:[-\w.]|(?:%[\da-fA-F]{2}))+""?$");
    }
}
