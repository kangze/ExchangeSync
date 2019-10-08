using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Base.Mdm.Org.MsgContracts;
using ExchangeSync.Model.EnterpiseContactModel;
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
