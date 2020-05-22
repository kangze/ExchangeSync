using ExchangeSync.Exchange.Model;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Microsoft.Exchange.WebServices.Data.Task;

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
            appointment.IsAllDayEvent = dto.FullDay;
            //appointment.ReminderDueBy
            if (!dto.FullDay)
                appointment.End = dto.End;
            else
                appointment.End = new DateTime(dto.Start.Year, dto.Start.Month, dto.Start.Day, 23, 59, 59);
            appointment.Location = dto.Location;
            foreach (var attachment in dto.Attachments)
            {
                var at = appointment.Attachments.AddFileAttachment(attachment.Name, attachment.Bytes);
                at.ContentId = attachment.Id;
                at.ContentType = "GIF/Image";
                at.IsInline = !attachment.IsPackage;//很重要
            }
            if (dto.Type == AppointMentType.Talk)
                dto.Attendees.ForEach(u => appointment.RequiredAttendees.Add(u));
            if (dto.Type == AppointMentType.Talk) await appointment.Save(SendInvitationsMode.SendToAllAndSaveCopy);
            else await appointment.Save(SendInvitationsMode.SendToNone);
            //Verify that the meeting is created
            Item item = await Item.Bind(this._exchangeService, appointment.Id, new PropertySet(ItemSchema.Subject));
            return appointment.Id.UniqueId;
        }

        /// <summary>
        /// 获取我的日历
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppointMentDto>> GetAppointMentsAsync()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddYears(2);
            var calendarFolder = await CalendarFolder.Bind(this._exchangeService, WellKnownFolderName.Calendar, new PropertySet());
            var cView = new CalendarView(startDate, endDate, int.MaxValue);
            cView.PropertySet = new PropertySet(
                ItemSchema.Id,
                ItemSchema.Subject,
                AppointmentSchema.Start,
                AppointmentSchema.End
                );
            FindItemsResults<Appointment> appointments = await calendarFolder.FindAppointments(cView);
            var list = new List<AppointMentDto>();
            foreach (var appointment in appointments)
            {
                var listItem = new AppointMentDto();
                await appointment.Load(new PropertySet(AppointmentSchema.TextBody, ItemSchema.Subject,
                AppointmentSchema.Start,
                ItemSchema.Id,
                AppointmentSchema.End,
                AppointmentSchema.Organizer,
                AppointmentSchema.RequiredAttendees,
                AppointmentSchema.OptionalAttendees));

                listItem.Id = appointment.Id.UniqueId;
                listItem.Organizer = appointment.Organizer.Name;
                listItem.Subject = appointment.Subject;
                listItem.Start = appointment.Start;
                listItem.End = appointment.End;
                listItem.Body = appointment.TextBody;
                listItem.Attendees = new List<string>();
                listItem.Attendees.AddRange(appointment.OptionalAttendees.Select(u => u.Name).ToList());
                listItem.Attendees.AddRange(appointment.RequiredAttendees.Select(u => u.Name).ToList());
                list.Add(listItem);
            }

            return list;
        }

        public async Task<bool> DeleteMeetAsyun(string id)
        {
            try
            {
                Appointment appointment = await Appointment.Bind(this._exchangeService, id, new PropertySet());
                await appointment.Delete(DeleteMode.MoveToDeletedItems);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }

        public async Task<string> GetAppointMentUrl(string id)
        {
            try
            {


                var calendarFolder = await CalendarFolder.Bind(this._exchangeService, WellKnownFolderName.Calendar,
                    new PropertySet());
                var searchFilter = new SearchFilter.IsEqualTo(ItemSchema.Id, id);
                var appointments = await calendarFolder.FindItems(searchFilter, new ItemView(1));
                var list = new List<AppointMentDto>();
                var appointment = appointments.First() as Appointment;
                if (appointment == null) return "";
                await appointment.Load(new PropertySet(
                    ItemSchema.Id,
                    AppointmentSchema.JoinOnlineMeetingUrl));
                return appointment.JoinOnlineMeetingUrl;
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
