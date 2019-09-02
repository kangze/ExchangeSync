using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IBindService
    {
        Task ConnectWithWeChatAsync(UserInfo userInfo, string password, string openId);
    }
}
