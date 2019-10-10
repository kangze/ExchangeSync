using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Model.ExchangeModel;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeSync.Exchange
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<EmailMessage, MailInfo>().
        }
    }
}
