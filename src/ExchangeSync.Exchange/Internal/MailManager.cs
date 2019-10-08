using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeSync.Model.ExchangeModel;
using Microsoft.Exchange.WebServices.Data;
using Task = System.Threading.Tasks.Task;
using System.Web;

namespace ExchangeSync.Exchange.Internal
{
    public class MailManager
    {
        private readonly string _userName;
        private readonly string _password;

        private readonly ExchangeService _exchangeService;

        private MailManager(string userName, string password)
        {
            this._userName = userName;
            this._password = password;
            this._exchangeService = this.Init();
        }

        private static Dictionary<string, MailManager> Managers { get; set; } = new Dictionary<string, MailManager>();

        public static MailManager Create(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(userName) + nameof(password));
            if (Managers.ContainsKey(userName))
                return Managers[userName];
            var manager = new MailManager(userName, password);
            Managers.Add(userName, manager);
            return manager;
        }

        private ExchangeService Init()
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
            service.Credentials = new WebCredentials(this._userName, this._password);
            service.UseDefaultCredentials = false;
#if DEBUG
            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.All;
#endif
            service.AutodiscoverUrl(this._userName, RedirectionUrlValidationCallback);
            return service;
        }

        public async Task SendMail(CreateMailModel model)
        {
            var message = new EmailMessage(this._exchangeService);
            message.ToRecipients.Add(model.TargetMail);
            message.Subject = model.Subject;
            message.Body = new MessageBody(model.Body);
            await message.Send();
        }

        /// <summary>
        /// 获取邮件消息
        /// </summary>
        public async Task<List<MailInfo>> GetMailMessageAsync()
        {
            PropertySet propSet = new PropertySet(BasePropertySet.IdOnly);
            Folder rootFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Inbox, propSet);
            var ss = await rootFolder.FindItems(new ItemView(10));
            var list = new List<MailInfo>();
            var regex = new Regex(@"<[^>]*>");
            foreach (var item in ss)
            {
                var mail = item as EmailMessage;
                if (mail == null) continue;
                await item.Load();
                var info = new MailInfo()
                {
                    Id = mail.Id.UniqueId,
                    Subject = mail.Subject,
                    Content = Regex.Replace(mail.Body.ToString(), @"<[^>]*>", ""),
                    RecivedTime = mail.DateTimeReceived,
                    Sender = mail.Sender.Address,
                    SenderName = mail.Sender.Name,
                    Attachments = mail.Attachments.Select(u => u.Name).ToList(),
                    Readed = mail.IsRead,
                };
                list.Add(info);
            }
            return list;
        }

        public async Task<MailInfo> GetMailAsync(string mailId)
        {
            mailId = HttpUtility.UrlDecode(mailId);
            PropertySet propSet = new PropertySet(BasePropertySet.IdOnly);
            Folder rootFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Inbox, propSet);
            var searchFilter = new SearchFilter.IsEqualTo(EmailMessageSchema.Id, mailId);
            var ss = await rootFolder.FindItems(searchFilter, new ItemView(1));
            var mail = ss.First() as EmailMessage;
            await mail.Load();
            var info = new MailInfo()
            {
                Id = mail.Id.UniqueId,
                Subject = mail.Subject,
                Content = mail.Body.ToString(),
                RecivedTime = mail.DateTimeReceived,
                Sender = mail.Sender.Address,
                SenderName = mail.Sender.Name,
                Attachments = mail.Attachments.Select(u => u.Name).ToList(),
                Readed = mail.IsRead,
            };
            return info;
        }



        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
}
