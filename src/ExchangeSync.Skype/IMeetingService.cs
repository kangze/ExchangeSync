using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public interface IMeetingService
    {
        Task<CreateMeetingResult> CreateOnlineMeetingAsync(string subject, string description, string account, string password);
    }
}
