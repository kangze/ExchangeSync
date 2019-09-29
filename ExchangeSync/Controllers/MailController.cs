using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class MailController : Controller
    {
        private readonly IMapper _mapper;

        public MailController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 获取本用户的邮件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAsync()
        {
            var service = MailManager.Create("v-ms-kz@scrbg.com", "tfs4418000");
            var dtos = await service.GetMailMessageAsync();
            var message = this._mapper.Map<IList<MailMessageViewModel>>(dtos);
            return Json(message);
        }

        /// <summary>
        /// 客户端发送邮件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PostAsync(NewMailInput input)
        {
            var service = MailManager.Create("", "");
            await service.SendMail(new CreateMailModel()
            {
                Subject = input.Subject,
                Body = input.Body,
                TargetMail = input.Recivers.FirstOrDefault()
            });
            return Ok("Ok");
        }
    }
}
