using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Extension
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).FirstOrDefault();
            var attr = member.GetCustomAttribute<DescriptionAttribute>();
            if (attr == null)
                return "";
            return attr.Description;
        }
    }
}
