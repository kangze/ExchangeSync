//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ExchangeSync.Model;
//using Microsoft.EntityFrameworkCore;

//namespace ExchangeSync.Services
//{
//    public class BindService : IBindService
//    {
//        private readonly ServiceDbContext _db;

//        public BindService(ServiceDbContext db)
//        {
//            _db = db;
//        }

//        public async Task ConnectWithWeChatAsync(UserInfo userInfo, string password, string openId)
//        {
//            var weChatbind = await _db.UserWeChats.FirstOrDefaultAsync(u => u.MdmId == userInfo.MdmId);
//            if (weChatbind != null) return;
//            await _db.UserWeChats.AddAsync(new UserWeChat()
//            {
//                MdmId = userInfo.MdmId,
//                OpenId = openId,
//                SsoId = userInfo.SsoId,
//                UserName = userInfo.UserName,
//                UserPassword = password,
//                BindTime = DateTimeOffset.Now
//            });
//            await _db.SaveChangesAsync();
//        }
//    }
//}
