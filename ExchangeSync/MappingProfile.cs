using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Base.Mdm.Org.MsgContracts;
using ExchangeSync.Exchange.Model;
using ExchangeSync.Model.EnterpiseContactModel;
using ExchangeSync.Model.ExchangeModel;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeSync
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrgUnitEntityMsg, Department>()
                .ForMember(u => u.Parent, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Positions = this.ConvertDbPositions(s, d);
                    var adminParent = s.Parents.FirstOrDefault(x => x.Type == OrgUnitParentType.Admin);
                    d.ParentId = adminParent?.Id;
                });
            CreateMap<PositionEntityMsg, Position>();
            CreateMap<ContactEntityMsg, Employee>()
                .ForMember(u => u.Positions, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Id = s.ContactId;
                    var primaryPosition = s.Positions.FirstOrDefault(x => x.IsPrimary);
                    d.PrimaryDepartmentId = primaryPosition?.OrgUnitId ?? Guid.Empty;
                    d.PrimaryPositionId = primaryPosition?.PositionId ?? Guid.Empty;
                    d.Positions = ConvertDbEmployeePosition(s, d);
                    if (string.IsNullOrEmpty(d.IdCardNo)) d.IdCardNo = "NA";
                });

            CreateMap<MailInfo, MailItemViewModel>()
                .ForMember(u => u.MailId, opt => opt.MapFrom(u => u.Id))
                .ForMember(u => u.Title, opt => opt.MapFrom(u => u.Subject))
                .ForMember(u => u.Description, opt => opt.MapFrom(u => u.Description))
                .ForMember(u => u.Readed, opt => opt.MapFrom(u => u.Readed))
                .ForMember(u => u.Sender, opt => opt.MapFrom(u => new MailContactViewModel()
                {
                    Name = u.SenderName,
                    Address = u.Sender,
                }))
                .ForMember(u => u.Recivers, opt => opt.MapFrom(u => u.Recivers.Select(x => new MailContactViewModel()
                {
                    Name = x.Name,
                    Address = x.Address
                }).ToList()))
                .ForMember(
                    u => u.HasAttachments, opt => opt.MapFrom(u => u.HasAttachments))
                .ForMember(u => u.Date, opt => opt.MapFrom(u => u.RecivedTime))
                .AfterMap((s, d) =>
                {
                    d.Date = s.RecivedTime.Month + "月" + s.RecivedTime.Day + "日";
                });

            CreateMap<MailInfo, MailDetailViewModel>()
                .ForMember(u => u.MailId, opt => opt.MapFrom(u => u.Id))
                .ForMember(u => u.Title, opt => opt.MapFrom(u => u.Subject))
                .ForMember(u => u.Content, opt => opt.MapFrom(u => u.Content))
                .ForMember(u => u.Sender, opt => opt.MapFrom(u => new MailContactViewModel()
                {
                    Name = u.SenderName,
                    Address = u.Sender,
                }))
                .ForMember(u => u.Recivers, opt => opt.MapFrom(u => u.Recivers.Select(x => new MailContactViewModel()
                {
                    Name = x.Name,
                    Address = x.Address
                }).ToList()))
                .ForMember(u => u.HasAttachments, opt => opt.MapFrom(u => u.HasAttachments))
                .ForMember(u => u.Attachments,
                    opt => opt.MapFrom(u => u.Attachments.Select(x => new MailAttachmentViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Size = x.Size
                    }).ToList()))
                .ForMember(u => u.Date, opt => opt.MapFrom(u => u.RecivedTime))
                .AfterMap((s, d) =>
                {
                    d.Date = s.RecivedTime.Month + "月" + s.RecivedTime.Day + "日";
                });

            CreateMap<AppointMenInput, AppointMentDto>()
                .ForMember(u => u.Attachments, opt => opt.Ignore())
                .ForMember(u => u.Subject, opt => opt.MapFrom(u => u.Title))
                .AfterMap((s, d) =>
                {
                    //全天事件
                    d.Start = DateTime.Parse(s.Start + " " + s.StartTime);
                    if (!s.FullDay)
                        d.End = DateTime.Parse(s.End + " " + s.EndTime);
                    else
                        d.Start = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, 0, 0, 0);
                });

        }

        private List<Position> ConvertDbPositions(OrgUnitEntityMsg s, Department d)
        {
            var result = new List<Position>();
            foreach (var positionEntityMsg in s.Positions)
            {
                var item = new Position()
                {
                    DepartmentId = s.Id,
                    Id = positionEntityMsg.Id,
                    Name = positionEntityMsg.Name,
                    Department = d
                };
                result.Add(item);
            }
            return result;
        }


        private List<EmployeePosition> ConvertDbEmployeePosition(ContactEntityMsg s, Employee d)
        {
            var result = new List<EmployeePosition>();
            foreach (var positionValueMsg in s.Positions)
            {
                var item = new EmployeePosition()
                {
                    Employee = d,
                    EmployeeId = s.ContactId,
                    IsPrimary = positionValueMsg.IsPrimary,
                    PositionId = positionValueMsg.PositionId,
                };
                result.Add(item);
            }
            return result;

        }
    }
}
