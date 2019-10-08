﻿using AutoMapper;
using Base.Mdm.Org.MsgContracts;
using EFCore.BulkExtensions;
using ExchangeSync.Model.EnterpiseContactModel;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Model.Consumers
{
    public class OrgEventDataConsumer : IConsumer<OrgDataEvent>
    {
        protected readonly IMapper _mapper;
        protected readonly DbContextOptions<ServiceDbContext> _dbOptions;

        public OrgEventDataConsumer(DbContextOptions<ServiceDbContext> dbOptions, IMapper mapper)
        {
            _mapper = mapper;
            _dbOptions = dbOptions;
        }

        public async Task Consume(ConsumeContext<OrgDataEvent> context)
        {
            var message = context.Message;
            await this.AddOrgUnits(message.OrgUnitAddeds);
            await this.UpdateOrgUnits(message.OrgUnitUpdateds);
            await this.DeleteOrgUnits(message.OrgUnitDeleteds);
            await this.AddContacts(message.ContactAddeds);
            await this.UpdateContacts(message.ContactUpdateds);
            await this.DeleteContacts(message.ContactDeleteds);

        }

        private async Task DeleteOrgUnits(List<OrgUnitDeletedMsg> adds)
        {
            var orgUnits = this._mapper.Map<IList<Department>>(adds.Select(u => u.OldData));
            var positons = orgUnits.SelectMany(u => u.Positions).ToList();
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkDeleteAsync(orgUnits);
                await db.BulkDeleteAsync(positons);
            }
        }

        private async Task UpdateOrgUnits(List<OrgUnitUpdatedMsg> updates)
        {
            var orgUnits = this._mapper.Map<IList<Department>>(updates.Select(u => u.OldData));
            var positons = orgUnits.SelectMany(u => u.Positions).ToList();
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkUpdateAsync(orgUnits);
                await db.BulkUpdateAsync(positons);
            }
        }

        private async Task AddOrgUnits(List<OrgUnitAddedMsg> adds)
        {
            var orgUnits = this._mapper.Map<IList<Department>>(adds.Select(u => u.NewData));
            var positons = orgUnits.SelectMany(u => u.Positions).ToList();
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkInsertOrUpdateAsync(orgUnits);
                await db.BulkInsertOrUpdateAsync(positons);
            }
        }

        /// <summary>
        /// 删除联系人,
        /// </summary>
        /// <param name="deletes"></param>
        /// <returns></returns>
        private async Task DeleteContacts(List<ContactDeletedMsg> deletes)
        {
            var employees = this._mapper.Map<IList<Employee>>(deletes.Select(u => u.OldData));
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkDeleteAsync(employees);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 更新联系人
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        private async Task UpdateContacts(List<ContactUpdatedMsg> updates)
        {
            var employees = this._mapper.Map<IList<Employee>>(updates.Select(u => u.NewData));
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkUpdateAsync(employees);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="adds"></param>
        /// <returns></returns>
        private async Task AddContacts(IList<ContactAddedMsg> adds)
        {
            var employees = this._mapper.Map<IList<Employee>>(adds.Select(u => u.NewData));
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.BulkInsertAsync(employees);
                await db.SaveChangesAsync();
            }
        }
    }
}
