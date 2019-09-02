using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Mdm.Org.MsgContracts;
using ExchangeSync.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Consumers
{
    public class HrConsumer : IConsumer<FullOrgData>
    {
        protected readonly DbContextOptions<ServiceDbContext> _dbOptions;

        public HrConsumer(DbContextOptions<ServiceDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task Consume(ConsumeContext<FullOrgData> context)
        {
            var message = context.Message;
            if (message.Contacts == null || message.Contacts.Count == 0) return;

            var users = ConvertToDbConnect(message.Contacts);
        }

        private object ConvertToDbConnect(List<ContactEntityMsg> contacts)
        {
            throw new NotImplementedException();
        }
    }
}
