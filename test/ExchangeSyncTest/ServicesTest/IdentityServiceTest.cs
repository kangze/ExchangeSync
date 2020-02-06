using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeSync.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeSyncTest.ServicesTest
{
    [TestClass]
    public class IdentityServiceTest
    {
        [TestMethod]
        public async Task GetUserAccessTokenAsync_Test()
        {



            var input = "<img alt=\"1.jpg\" src=\"http://ww.b.com/1\">";
            Regex re = new Regex("(?i)<img(?=[^>]*?alt=([\"']?)(?<alt>(?:(?!\\1).)*)\\1)[^>]*?src=([\"']?)(?<src>(?:(?!\\2).)*)\\2[^>]+>");
            var match = re.Match(input);
            //var service = new IdentityService(new HttpClient(), new IdOptions(new IdSvrOption()
            //{
            //    IssuerUri = "https://login.scrbg.com",
            //    RequireHttps = true,
            //    ClientId = "OM_BI_PORTAL_Web_001",
            //    ClientSecret = "OMBIPORTALWeb001",

            //}));
            var userName = "003139";
            var password = "a123456";
            //var access_token = await service.GetUserAccessTokenAsync(userName, password, "openid profile profile.ext");
            //var userInfo = await service.GetUserInfoAsync(access_token);
            //Assert.IsNotNull(access_token);
            //Assert.IsNotNull(userInfo);
        }

        [TestMethod]
        public void TestUserNumber()
        {
            var number = this.OpenS("9gsaQOytp11mcDxAC6TgyYdM45ubvqQy");
            Assert.IsTrue(number== "000078");
        }

        public string OpenS(string str)
        {
            try
            {
                byte[] key = Encoding.Unicode.GetBytes("sclq");//密钥
                byte[] data = Convert.FromBase64String(str);//待解密字符串

                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
                MemoryStream MStream = new MemoryStream();//内存流对象

                //用内存流实例化解密流对象
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
                CStream.Write(data, 0, data.Length);//向加密流中写入数据
                CStream.FlushFinalBlock();//将数据压入基础流
                byte[] temp = MStream.ToArray();//从内存流中获取字节序列
                CStream.Close();//关闭加密流
                MStream.Close();//关闭内存流

                return Encoding.Unicode.GetString(temp);//返回解密后的字符串
            }
            catch
            {
                return str;
            }
        }
    }

    public class IdOptions : IOptions<IdSvrOption>
    {
        public IdOptions(IdSvrOption option)
        {
            this.Value = option;
        }

        public IdSvrOption Value { get; }
    }
}
