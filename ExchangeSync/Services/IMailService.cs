using ExchangeSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeSync.Services
{
    public interface IMailService
    {
        Task<List<MailIndexViewModel>> GetIndexMailAsync(string identity);

        Task<MailIndexItemViewModel> GetMailAsync(string mailId);

        Task<List<MailIndexViewModel>> GetSendedMailAsync(string identity);

        Task<List<MailIndexViewModel>> GetDraftMailAsync(string identity);
    }
}
