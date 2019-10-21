using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Helper;
using ExchangeSync.Model.ExchangeModel;
using ExchangeSync.Models;
using Newtonsoft.Json;

namespace ExchangeSync.Services
{
    public class MailService : IMailService
    {
        private readonly IMapper _mapper;

        public static string TestAccount = "scbzzx@scrbg.com";
        public static string TestPassword = "a123456";
        public static string TestName = "王力为";
        public MailService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 草稿箱
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<List<MailGroupViewModel>> GetDraftMailAsync(string identity)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var drafts = await mailManager.GetDraftMailAsync();
            var result = this.GroupMail(drafts);
            foreach (var item in result.SelectMany(u => u.Items))
            {
                item.Sender = new MailContactViewModel()
                {
                    Name = TestName,
                    Address = TestAccount,
                };
            }
            return result;
        }

        /// <summary>
        /// 获取收件箱内容
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<List<MailGroupViewModel>> GetIndexMailAsync(string identity)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var indexs = await mailManager.GetInBoxMessageAsync();
            var mails = this.GroupMail(indexs);
            return mails;
        }

        /// <summary>
        /// 获取一个具体的邮件相关的信息
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        public async Task<MailDetailViewModel> GetMailAsync(string mailId)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var result = await mailManager.GetMailAsync(mailId);
            return this._mapper.Map<MailDetailViewModel>(result);
        }

        /// <summary>
        /// 已经发送的邮件
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<List<MailGroupViewModel>> GetSendedMailAsync(string identity)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var sents = await mailManager.GetSentMailMessageAsync();
            var mails = this.GroupMail(sents);
            return mails;
        }

        private List<MailGroupViewModel> GroupMail(List<MailInfo> mails)
        {
            //定义时间线,本周,上周,上月,直到1月,然后更早
            var startWeek = DateTimeHelper.GetStartWeek(DateTime.Now);
            var endWeek = DateTimeHelper.GetEndWeek(DateTime.Now);
            var lastStartWeek = DateTimeHelper.GetLastStartWeek(DateTime.Now);
            var lastEndWeek = DateTimeHelper.GetLastEndWeek(DateTime.Now);
            var thisMonth = DateTime.Now.Month;
            var lastMonth = thisMonth - 1;
            List<MailGroupViewModel> ls = new List<MailGroupViewModel>();
            var thisWeekData = mails.Where(u => u.RecivedTime >= startWeek && u.RecivedTime <= endWeek).ToList();
            if (thisWeekData.Count != 0)
            {
                mails.RemoveAll(u => thisWeekData.Contains(u));
                ls.Add(new MailGroupViewModel()
                {
                    GroupTitle = "本周",
                    Items = this._mapper.Map<List<MailItemViewModel>>(thisWeekData)
                });
            }

            var lastWeekData = mails.Where(u => u.RecivedTime >= lastStartWeek && u.RecivedTime <= lastEndWeek)
                .ToList();
            if (lastWeekData.Count != 0)
            {
                mails.RemoveAll(u => lastWeekData.Contains(u));
                ls.Add(new MailGroupViewModel()
                {
                    GroupTitle = "上周",
                    Items = this._mapper.Map<List<MailItemViewModel>>(lastWeekData)
                });
            }

            var thisMonthData = mails.Where(u => u.RecivedTime.Month == thisMonth).ToList();
            if (thisMonthData.Count != 0)
            {
                mails.RemoveAll(u => thisMonthData.Contains(u));
                ls.Add(new MailGroupViewModel()
                {
                    GroupTitle = "本月",
                    Items = this._mapper.Map<List<MailItemViewModel>>(thisMonthData)
                });
            }

            var lastMonthData = mails.Where(u => u.RecivedTime.Month == lastMonth).ToList();
            if (lastMonthData.Count != 0)
            {
                mails.RemoveAll(u => lastMonthData.Contains(u));
                ls.Add(new MailGroupViewModel()
                {
                    GroupTitle = "上月",
                    Items = this._mapper.Map<List<MailItemViewModel>>(lastMonthData)
                });
            }

            if (mails.Count != 0)
            {
                for (var i = lastMonth - 1; i <= 1; i--)
                {
                    var items = mails.Where(u => u.RecivedTime.Month == i).ToList();
                    ls.Add(new MailGroupViewModel()
                    {
                        GroupTitle = i + "月",
                        Items = this._mapper.Map<List<MailItemViewModel>>(items),
                    });
                    mails.RemoveAll(u => items.Contains(u));
                }
            }

            if (mails.Count != 0)
            {
                ls.Add(new MailGroupViewModel()
                {
                    GroupTitle = "更早",
                    Items = this._mapper.Map<List<MailItemViewModel>>(mails),
                });
            }

            return ls;
        }

        public async Task ReplyAsync(string mailId, string content)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            await mailManager.Reply(mailId, content);
        }

        public async Task<Stream> Download(string mailId, string attachmentId)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var stream = await mailManager.DownLoadAttachment(mailId, attachmentId);
            return stream;
        }

        public async Task Send(string title, string content, string reciver)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            await mailManager.SendMail(new Exchange.CreateMailModel()
            {
                Body = content,
                Subject = title,
                TargetMail = reciver
            });
        }

    }
}
