using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Helper;
using ExchangeSync.Models;
using Newtonsoft.Json;

namespace ExchangeSync.Services
{
    public class MailService : IMailService
    {
        //private readonly IMapper _mapper;

        public MailService()
        {
            //_mapper = mapper;
        }

        public async Task<List<MailIndexViewModel>> GetDraftMailAsync(string identity)
        {
            var json = "";// JsonConvert.SerializeObject(MailIndexs);
            var data = JsonConvert.DeserializeObject<List<MailIndexViewModel>>(json);
            foreach (var item in data.SelectMany(u => u.Items))
            {
                item.Title = "[草稿]" + item.Title;
            }
            return await Task.FromResult(data);
        }

        /// <summary>
        /// 获取收件箱内容
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<List<MailIndexViewModel>> GetIndexMailAsync(string identity)
        {
            var mailManager = MailManager.Create("v-ms-kz@scrbg.com", "tfs4418000");
            var result = await mailManager.GetInBoxMessageAsync();
            var result2 = await mailManager.GetSentMailMessageAsync();
            var result3 = await mailManager.GetDraftMailAsync();
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
                    //Attachments = u.Attachments,
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
                    //Attachments = u.Attachments
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
                //Attachments = result.Attachments
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
            var json = ""; // JsonConvert.SerializeObject(MailIndexs);
            var data = JsonConvert.DeserializeObject<List<MailIndexViewModel>>(json);
            foreach (var item in data.SelectMany(u => u.Items))
            {
                item.Title = "[已发送]" + item.Title;
            }
            return await Task.FromResult(data);
        }
    }
}
