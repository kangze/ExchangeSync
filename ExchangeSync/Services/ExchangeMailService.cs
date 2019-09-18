using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Model;
using ExchangeSync.Services.Dtos;
using ExchangeSync.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Services
{
    public class ExchangeMailService : IExchangeMailService
    {
        private readonly MailManager _mailManager;
        private readonly ServiceDbContext _db;
        private readonly IMapper _mapper;

        public ExchangeMailService(MailManager mailManager,
            ServiceDbContext db,
            IMapper mapper)
        {
            this._mailManager = mailManager;
            this._db = db;
            this._mapper = mapper;
        }

        public async Task SendAsync(NewMailInput input)
        {
            if (input == null) throw new ArgumentException(nameof(input));

            var user = await this._db.UserSecrets.FirstOrDefaultAsync(u => u.UserAccount == input.Sender);
            if (user == null) throw new AccountNotFoundException(input.Sender);

            var mailManager = MailManager.Create(user.UserAccount, user.Password);
            await mailManager.SendMail(this._mapper.Map<CreateMailModel>(input));
        }
    }
}
