using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Models
{
    public enum ConnectStatusCode
    {
        [Description("用户名或者密码不能为空")]
        AccountOrPasswordEmpty = 0,

        [Description("用户名错误")]
        AccountError = 1,

        [Description("密码错误")]
        PasswordError = 2,

        [Description("账户被禁用")]
        AccountForbidden = 3,

        [Description("账户被锁定")]
        AccountFreeze = 4
    }
}
