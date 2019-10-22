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
            input.Body = MailService.ConverToHtml(input.Body);
            var dto = this._mapper.Map<AppointMentDto>(input);
            dto.Attachments = new List<Exchange.AttachmentMailModel>();
            foreach (var item in input.Attachments)
            {
                dto.Attachments.Add(new Exchange.AttachmentMailModel()
                {
                    Bytes = item.Bytes,
                    Name = item.Name,
                    Id = item.Id,
                    IsPackage=item.IsPackage
                });
            }
            await mailManager.CreateAppointMentAsync(dto);
        }

        public Task<List<AppointMentDto>> GetMyAppointmentsAsync()
        {
            var mailManager = MailManager.Create(TestAccount, TestPassword);
            var list = mailManager.GetAppointMentsAsync();
            return list;
        }
    }
}
