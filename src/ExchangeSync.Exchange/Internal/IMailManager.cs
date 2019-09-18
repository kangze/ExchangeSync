using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model.ExchangeModel;

namespace ExchangeSync.Exchange.Internal
{
    public interface IMailManager
    {
        void SendMail(CreateMailModel model);

        Task<List<MailInfo>> GetMailMessageAsync();
    }
}
