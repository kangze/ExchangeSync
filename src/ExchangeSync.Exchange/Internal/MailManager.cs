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
using AutoMapper;
using System.IO;

namespace ExchangeSync.Exchange.Internal
{
    public partial class MailManager
    {
        private readonly string _userName;
        private readonly string _password;

        private readonly ExchangeService _exchangeService;
        private readonly IMapper _mapper;

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
            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                foreach (var attachment in model.Attachments)
                {
                    message.Attachments.AddFileAttachment(attachment.Name, attachment.Bytes);
                }
            }

            await message.SendAndSaveCopy();
        }

        /// <summary>
        /// 获取收件箱的邮件
        /// </summary>
        private async Task<List<MailInfo>> GetEmailMessageListAsync(WellKnownFolderName name)
        {
            PropertySet propSet = new PropertySet(new List<PropertyDefinitionBase>()
            {
                ItemSchema.Id,
                ItemSchema.Subject,
                ItemSchema.DateTimeReceived,
                ItemSchema.DateTimeCreated,
                ItemSchema.DateTimeSent,
                EmailMessageSchema.Sender,
                ItemSchema.HasAttachments,
                EmailMessageSchema.IsRead,
                ItemSchema.TextBody,
                ItemSchema.DisplayCc,
                ItemSchema.DisplayTo,
            });
            Folder rootFolder = await Folder.Bind(this._exchangeService, name, propSet);
            //TODO:先设计成获取前200封邮件
            var mailItems = await rootFolder.FindItems(new ItemView(200));
            var list = new List<MailInfo>();
            foreach (var item in mailItems)
            {
                var mail = item as EmailMessage;
                if (mail == null) continue;
                await mail.Load(propSet);
                list.Add(this.ConvertToMailInfo(mail, propSet));
            }
            return list;
        }

        public async Task<List<MailInfo>> GetInBoxMessageAsync()
        {
            var result = await this.GetEmailMessageListAsync(WellKnownFolderName.Inbox);
            return result;
        }

        public async Task<List<MailInfo>> GetSentMailMessageAsync()
        {
            var result = await this.GetEmailMessageListAsync(WellKnownFolderName.SentItems);
            return result;
        }

        public async Task<List<MailInfo>> GetDraftMailAsync()
        {
            var result = await this.GetEmailMessageListAsync(WellKnownFolderName.Drafts);
            return result;
        }

        public async Task<MailInfo> GetMailAsync(string mailId)
        {
            mailId = HttpUtility.UrlDecode(mailId);
            mailId = mailId.Replace(' ', '+');
            PropertySet propSet = new PropertySet(new List<PropertyDefinitionBase>()
            {
                ItemSchema.Id,
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.DateTimeReceived,
                EmailMessageSchema.Sender,
                ItemSchema.Attachments,
                EmailMessageSchema.IsRead,
                ItemSchema.DisplayCc,
                ItemSchema.DisplayTo
            });
            Folder rootFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Inbox, propSet);
            var searchFilter = new SearchFilter.IsEqualTo(ItemSchema.Id, mailId);
            var mailItems = await rootFolder.FindItems(searchFilter, new ItemView(1));
            if (mailItems.Any())
            {
                var mail = mailItems.First() as EmailMessage;
                if (mail == null) return null;
                await mail.Load(propSet);
                return this.ConvertToMailInfo(mail, propSet);
            }

            Folder sentFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.SentItems, propSet);
            var sentmailItems = await sentFolder.FindItems(searchFilter, new ItemView(1));
            if (sentmailItems.Any())
            {
                var mail = sentmailItems.First() as EmailMessage;
                if (mail == null) return null;
                await mail.Load(propSet);
                return this.ConvertToMailInfo(mail, propSet);
            }

            Folder draftrootFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Drafts, propSet);
            var draftmailItems = await draftrootFolder.FindItems(searchFilter, new ItemView(1));
            if (draftmailItems.Any())
            {
                var mail = draftmailItems.First() as EmailMessage;
                if (mail == null) return null;
                await mail.Load(propSet);
                return this.ConvertToMailInfo(mail, propSet);
            }
            return null;
        }

        public async Task<MemoryStream> DownLoadAttachment(string mailId, string attachmentId)
        {
            mailId = HttpUtility.UrlDecode(mailId);
            mailId = mailId.Replace(' ', '+');
            attachmentId = HttpUtility.UrlDecode(attachmentId);
            attachmentId = attachmentId.Replace(' ', '+');
            EmailMessage message = await EmailMessage.Bind(this._exchangeService, new ItemId(mailId), new PropertySet(ItemSchema.Attachments));
            // Iterate through the attachments collection and load each attachment.
            var attachment = message.Attachments.Where(u => u.Id.Equals(attachmentId, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (attachment == null) return null;
            MemoryStream ms = new MemoryStream();
            await attachment.Load();
            ms.Write((attachment as FileAttachment).Content);
            return ms;
        }

        public async Task SetReaded(string mailId, bool readed)
        {
            EmailMessage message = await EmailMessage.Bind(this._exchangeService, new ItemId(mailId), new PropertySet(ItemSchema.IsResend));
            if (message.IsRead == readed)
                return;
            message.IsRead = readed;
            await message.Update(ConflictResolutionMode.AutoResolve);
        }

        public async Task DeleteMail(string mailId)
        {
            EmailMessage mail = await EmailMessage.Bind(this._exchangeService, new ItemId(mailId), PropertySet.IdOnly);
            await mail.Delete(DeleteMode.MoveToDeletedItems);
        }

        private MailInfo ConvertToMailInfo(EmailMessage message, PropertySet sets)
        {
            if (message == null || sets == null) return null;
            var mail = new MailInfo();
            if (sets.Contains(ItemSchema.Id)) mail.Id = message.Id.UniqueId;
            if (sets.Contains(ItemSchema.Subject)) mail.Subject = message.Subject;
            if (sets.Contains(ItemSchema.Body)) mail.Content = message.Body.ToString();
            if (sets.Contains(ItemSchema.TextBody)) mail.Description = message.TextBody.ToString();
            if (sets.Contains(ItemSchema.DateTimeReceived)) mail.RecivedTime = message.DateTimeReceived;
            if (sets.Contains(ItemSchema.DateTimeCreated)) mail.CreateTime = message.DateTimeCreated;
            if (sets.Contains(ItemSchema.DateTimeSent)) mail.SentTime = message.DateTimeSent;
            if (sets.Contains(EmailMessageSchema.Sender))
            {
                if (message.Sender != null)
                {
                    mail.Sender = message.Sender.Address;
                    mail.SenderName = message.Sender.Name;
                }
            }
            if (sets.Contains(ItemSchema.HasAttachments)) mail.HasAttachments = message.HasAttachments;
            if (sets.Contains(ItemSchema.Attachments))
                mail.Attachments = message.Attachments.Where(u => u is FileAttachment)
                    .Select(u => new AttachmentInfo() { Id = u.Id, Name = u.Name, Size = u.Size }).ToList();
            if (sets.Contains(EmailMessageSchema.IsRead))
                mail.Readed = message.IsRead;
            if (sets.Contains(ItemSchema.DisplayTo))
            {
                var to = message.DisplayTo.Split(";", StringSplitOptions.RemoveEmptyEntries);
                mail.Recivers = to.Select(u =>
                {
                    if (true) //是一个人的名字
                        return new EmailContact() { Name = u, Address = "" };
                    return new EmailContact() { Name = "", Address = u };
                }).ToList();
            }

            if (sets.Contains(ItemSchema.DisplayCc))
            {
                var cc = message.DisplayTo.Split(";", StringSplitOptions.RemoveEmptyEntries);
                mail.Cc = cc.Select(u =>
                {
                    if (true) //是一个人的名字
                        return new EmailContact() { Name = u, Address = "" };
                    return new EmailContact() { Name = "", Address = u };
                }).ToList();
            }
            return mail;
        }

        public async Task Reply(string mailId, string content)
        {
            mailId = HttpUtility.UrlDecode(mailId);
            mailId = mailId.Replace(' ', '+');
            Folder rootFolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Inbox, BasePropertySet.IdOnly);
            var searchFilter = new SearchFilter.IsEqualTo(ItemSchema.Id, mailId);
            var mailItems = await rootFolder.FindItems(searchFilter, new ItemView(1));
            if (mailItems.Count() == 0)
                return;
            var mail = mailItems.First() as EmailMessage;
            if (mail is null) return;
            await mail.Reply(new MessageBody(BodyType.HTML, content), true);
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
