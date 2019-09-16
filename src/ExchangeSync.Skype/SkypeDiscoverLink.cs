using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public class SkypeDiscoverLink
    {
        /// <summary>
        /// address from _links:self
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// address from _links:user
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// address from _links:xframe
        /// </summary>
        public string XFrame { get; set; }
    }
}
