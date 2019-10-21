using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Models.Inputs;

namespace ExchangeSync.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IMapper _mapper;

        public static string TestAccount = "scbzzx@scrbg.com";
        public static string TestPassword = "a123456";
        public static string TestName = "王力为";

        public CalendarService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateAppointMentAsync(AppointMenInput input)
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            await mailManager.CreateAppointMentAsync(this._mapper.Map<AppointMentDto>(input));
        }

        public Task<List<AppointMentDto>> GetMyAppointmentsAsync()
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var list = mailManager.GetAppointMentsAsync();
            return list;
        }
    }
}
