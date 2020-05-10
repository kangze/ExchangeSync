using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Mdm.Org.MsgContracts;
using EFCore.BulkExtensions;
using ExchangeSync.Model;
using ExchangeSync.Model.EnterpiseContactModel;
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

            ConvertToDbConnect(message.Contacts);
        }

        private void ConvertToDbConnect(List<ContactEntityMsg> contacts)
        {
            var employees = new List<Employee>();
            foreach (var contact in contacts)
            {
                var employee = new Employee()
                {
                    Id = contact.ContactId,
                    UserName = contact.UserName,
                    UserId = contact.UserId,
                    Name = contact.Name,
                    Number = contact.Number,
                    IdCardNo = contact.IdCardNo,
                    Gender = contact.Gender,
                };
                employees.Add(employee);
            }

            using (var db = new ServiceDbContext(_dbOptions))
            {
                db.BulkInsertOrUpdateOrDelete(employees);
            }
        }
    }
}
