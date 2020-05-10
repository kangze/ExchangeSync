using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Models.Inputs;

namespace ExchangeSync.Services
{
    public interface ICalendarService
    {
        Task<string> CreateAppointMentAsync(AppointMenInput input, string username, string password);

        Task<List<AppointMentDto>> GetMyAppointmentsAsync(string username, string password);

        Task<bool> DeleteMeetingAsync(string account, string password, string id);
    }
}
