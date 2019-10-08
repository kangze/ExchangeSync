using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Helper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取给定时间的周一
        /// </summary>
        /// <returns></returns>
        public static DateTime GetStartWeek(DateTime dt)
        {
            var dayOfWeek = 1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            if (dayOfWeek == 1) dayOfWeek = -6;

            DateTime startWeek = dt.AddDays(dayOfWeek);  //本周周一
            return new DateTime(startWeek.Year, startWeek.Month, startWeek.Day, 0, 0, 0);
        }

        /// <summary>
        /// 获取给定的时间的周末
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndWeek(DateTime dt)
        {
            var startWeek = GetStartWeek(dt);
            DateTime endWeek = startWeek.AddDays(6);  //本周周日
            return new DateTime(endWeek.Year, endWeek.Month, endWeek.Day, 23, 59, 59);
        }

        /// <summary>
        /// 获取上周周一
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastStartWeek(DateTime dt)
        {
            //上周的开始时间
            var temp = GetStartWeek(dt).AddDays(-1);
            return GetStartWeek(temp);
        }

        public static DateTime GetLastEndWeek(DateTime dt)
        {
            //上周的开始时间
            var temp = GetStartWeek(dt).AddDays(-1);
            return GetEndWeek(temp);
        }
    }
}
