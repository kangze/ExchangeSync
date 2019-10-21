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
        Task CreateAppointMentAsync(AppointMenInput input);

        Task<List<AppointMentDto>> GetMyAppointmentsAsync();
    }
}
