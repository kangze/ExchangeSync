﻿using ExchangeSync.Exchange.Model;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Exchange.Internal
{
    public partial class MailManager
    {

        public async Task<string> CreateAppointMentAsync(AppointMentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var appointment = new Appointment(this._exchangeService);
            appointment.Subject = dto.Subject;
            appointment.Body = dto.Body;
            appointment.Start = dto.Start;
            appointment.End = dto.End;
            appointment.Location = dto.Location;
            if (dto.Type == AppointMentType.Talk)
                dto.Attendees.ForEach(u => appointment.RequiredAttendees.Add(u));
            if (dto.Type == AppointMentType.Talk) await appointment.Save(SendInvitationsMode.SendToAllAndSaveCopy);
            else await appointment.Save(SendInvitationsMode.SendToNone);
            //Verify that the meeting is created
            Item item = await Item.Bind(this._exchangeService, appointment.Id, new PropertySet(ItemSchema.Subject));
            return appointment.Id.UniqueId;
        }
    }
}