using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Common.Tools
{
    public static class Check
    {
        public static void NotNull(this object obj, string argumentName = "arg")
        {
            if (obj == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}
