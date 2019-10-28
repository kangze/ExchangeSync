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

        public CalendarService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateAppointMentAsync(AppointMenInput input, string username, string password)
        {
            var mailManager = MailManager.Create(username, password);
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
                    IsPackage = item.IsPackage
                });
            }
            await mailManager.CreateAppointMentAsync(dto);
        }

        public Task<List<AppointMentDto>> GetMyAppointmentsAsync(string username, string password)
        {
            var mailManager = MailManager.Create(username, password);
            var list = mailManager.GetAppointMentsAsync();
            return list;
        }
    }
}
