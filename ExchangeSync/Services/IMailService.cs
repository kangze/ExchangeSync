using ExchangeSync.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IMailService
    {
        Task<List<MailGroupViewModel>> GetIndexMailAsync(string identity);

        Task<MailDetailViewModel> GetMailAsync(string mailId);

        Task<List<MailGroupViewModel>> GetSendedMailAsync(string identity);

        Task<List<MailGroupViewModel>> GetDraftMailAsync(string identity);

        Task ReplyAsync(string mailId, string conent);

        Task<Stream> Download(string mailId, string attachmentId);

        Task Send(string title, string content, string reciver);
    }
}
