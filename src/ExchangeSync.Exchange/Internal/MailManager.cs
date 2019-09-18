using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model.ExchangeModel;
using Microsoft.Exchange.WebServices.Data;
using Task = System.Threading.Tasks.Task;

namespace ExchangeSync.Exchange.Internal
{
    public class MailManager
    {
        private readonly string _userName;
        private readonly string _password;

        private readonly ExchangeService _exchangeService;

        public MailManager(string userName, string password)
        {
            this._userName = userName;
            this._password = password;
            this._exchangeService = this.Init();
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
            Folder rootfolder = await Folder.Bind(this._exchangeService, WellKnownFolderName.Inbox, propSet);
            var ss = await rootfolder.FindItems(new ItemView(10));
            var list = new List<MailInfo>();
            foreach (var item in ss)
            {
                await item.Load();
                var info = new MailInfo()
                {
                    Subject = item.Subject,
                    Content = item.Body.ToString(),
                    SendedTime = DateTimeOffset.MaxValue
                };
                list.Add(info);
            }

            return list;
        }



        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
}
