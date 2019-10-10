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

namespace ExchangeSync.Exchange.Internal
{
    public class MailManager
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
            await message.Send();
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
            if (!mailItems.Any()) return null;
            var mail = mailItems.First() as EmailMessage;
            if (mail == null) return null;
            return this.ConvertToMailInfo(mail, propSet);
        }

        public async Task<MailInfo> DownLoadAttachment(string mailId, string attachmentId)
        {
            EmailMessage message = await EmailMessage.Bind(this._exchangeService, new ItemId(mailId), new PropertySet(ItemSchema.Attachments));
            // Iterate through the attachments collection and load each attachment.
            foreach (Attachment attachment in message.Attachments)
            {
                if (attachment is FileAttachment)
                {
                    FileAttachment fileAttachment = attachment as FileAttachment;
                    // Load the attachment into a file.
                    // This call results in a GetAttachment call to EWS.
                    // DirectoryInfo directory = Directory.CreateDirectory("C:\\temp\\");
                    fileAttachment.Load("C:\\temp\\" + fileAttachment.Name);

                }
            }
            return null;
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
                mail.Attachments = message.Attachments
                    .Select(u => new AttachmentInfo() { Id = u.Id, Name = u.Name, Size = u.Size }).ToList();
            if (sets.Contains(EmailMessageSchema.IsRead))
                mail.Readed = mail.Readed;
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
