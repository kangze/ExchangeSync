﻿using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
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

        Task<MailDetailViewModel> GetMailAsync(string identity,string mailId);

        Task<List<MailGroupViewModel>> GetSendedMailAsync(string identity);

        Task<List<MailGroupViewModel>> GetDraftMailAsync(string identity);

        Task ReplyAsync(string identity,string mailId, string conent, string[] Cc, List<AttachmentInput> attachments);

        Task<Stream> Download(string identity,string mailId, string attachmentId);

        Task Send(string identity,string title, string content, string[] reciver, string[] cc, List<AttachmentInput> attachments);

        Task SetUnReade(string identity,string mailId);

        Task Delete(string identity,string mailId);
    }
}
