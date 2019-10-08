using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Helper;
using ExchangeSync.Models;
using Newtonsoft.Json;

namespace ExchangeSync.Services
{
    public class MailService : IMailService
    {
        public static string mailId1 { get; set; } = Guid.NewGuid().ToString();
        public static string mailId2 { get; set; } = Guid.NewGuid().ToString();
        public static string mailId3 { get; set; } = Guid.NewGuid().ToString();
        public static string mailId4 { get; set; } = Guid.NewGuid().ToString();

        public static List<MailIndexViewModel> MailIndexs = new List<MailIndexViewModel>()
            {
                new MailIndexViewModel()
                {
                    GroupTitle="本周",
                    Items=new List<MailIndexItemViewModel>
                    {
                        new MailIndexItemViewModel()
                        {
                            MailId=mailId1,
                            Title="我想约你",
                            Readed=true,
                            Description="其实我喜欢你很牛很牛很牛了你知道了嘛哈哈哈哈哈",
                            Date="周三",
                            Sender="kangze@scrbg.com",
                            SenderName="康泽",
                        },
                        new MailIndexItemViewModel()
                        {
                            MailId=mailId2,
                            Title="今天早上天气真的很不错哟",
                            Readed=false,
                            Description="其实我喜欢你很牛很牛很牛了你知道了嘛哈哈哈哈哈",
                            Date="周四",
                            Sender="kangze@scrbg.com",
                            SenderName="康泽",
                        }
                    }
                },
                 new MailIndexViewModel()
                {
                    GroupTitle="上周",
                    Items=new List<MailIndexItemViewModel>
                    {
                        new MailIndexItemViewModel()
                        {
                            MailId=mailId3,
                            Title="我想约你",
                            Readed=false,
                            Description="其实我喜欢你很牛很牛很牛了你知道了嘛哈哈哈哈哈",
                            Date="周三",
                            Sender="kangze@scrbg.com",
                            SenderName="康泽",
                        },
                        new MailIndexItemViewModel()
                        {
                            MailId=mailId4,
                            Title="今天早上天气真的很不错哟",
                            Readed=false,
                            Description="其实我喜欢你很牛很牛很牛了你知道了嘛哈哈哈哈哈",
                            Date="周四",
                            Sender="kangze@scrbg.com",
                            SenderName="康泽",
                        }
                    }
                }
            };
        public async Task<List<MailIndexViewModel>> GetDraftMailAsync(string identity)
        {
            var json = JsonConvert.SerializeObject(MailIndexs);
            var data = JsonConvert.DeserializeObject<List<MailIndexViewModel>>(json);
            foreach (var item in data.SelectMany(u => u.Items))
            {
                item.Title = "[草稿]" + item.Title;
            }
            return await Task.FromResult(data);
        }

        public async Task<List<MailIndexViewModel>> GetIndexMailAsync(string identity)
        {
            var mailManager = MailManager.Create("v-ms-kz@scrbg.com", "tfs4418000");
            var result = await mailManager.GetMailMessageAsync();
            List<MailIndexViewModel> ls = new List<MailIndexViewModel>();
            var startWeek = DateTimeHelper.GetStartWeek(DateTime.Now);
            var endWeek = DateTimeHelper.GetEndWeek(DateTime.Now);
            var lastStartWeek = DateTimeHelper.GetLastStartWeek(DateTime.Now);
            var lastEndWeek = DateTimeHelper.GetLastEndWeek(DateTime.Now);
            var thisWeekData = result.Where(u => u.RecivedTime >= startWeek && u.RecivedTime <= endWeek).ToList();
            var lastWeekData = result.Where(u => u.RecivedTime >= lastStartWeek && u.RecivedTime <= lastEndWeek).ToList();
            ls.Add(new MailIndexViewModel()
            {
                GroupTitle = "本周",
                Items = thisWeekData.Select(u => new MailIndexItemViewModel()
                {
                    MailId = u.Id,
                    Title = u.Subject,
                    Description = u.Content,
                    Date = "本周",
                    Sender = u.Sender,
                    SenderName = u.SenderName,
                    Readed = u.Readed,
                    Attachments = u.Attachments,
                }).ToList()
            });
            ls.Add(new MailIndexViewModel()
            {
                GroupTitle = "上周",
                Items = thisWeekData.Select(u => new MailIndexItemViewModel()
                {
                    Title = u.Subject,
                    Description = u.Content,
                    Date = "上周",
                    Sender = u.Sender,
                    SenderName = u.SenderName,
                    Readed = u.Readed,
                    Attachments = u.Attachments
                }).ToList()
            });
            return ls;
        }

        /// <summary>
        /// 获取一个具体的邮件相关的信息
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        public async Task<MailIndexItemViewModel> GetMailAsync(string mailId)
        {
            var mailManager = MailManager.Create("v-ms-kz@scrbg.com", "tfs4418000");
            var result = await mailManager.GetMailAsync(mailId);
            return new MailIndexItemViewModel()
            {
                MailId = result.Id,
                Title = result.Subject,
                Description = result.Content,
                Date = "上周",
                Sender = result.Sender,
                SenderName = result.SenderName,
                Readed = result.Readed,
                Attachments = result.Attachments
            };
        }

        //public async Task<MailIndexItemViewModel> GetMailAsync(string mailId)
        //{
        //    var mails = MailIndexs.SelectMany(u => u.Items);
        //    var item = mails.FirstOrDefault(u => u.MailId.Equals(mailId, StringComparison.CurrentCultureIgnoreCase));
        //    return await Task.FromResult(item);
        //}

        public async Task<List<MailIndexViewModel>> GetSendedMailAsync(string identity)
        {
            var json = JsonConvert.SerializeObject(MailIndexs);
            var data = JsonConvert.DeserializeObject<List<MailIndexViewModel>>(json);
            foreach (var item in data.SelectMany(u => u.Items))
            {
                item.Title = "[已发送]" + item.Title;
            }
            return await Task.FromResult(data);
        }
    }
}
