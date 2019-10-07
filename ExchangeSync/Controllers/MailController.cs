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
using ExchangeSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class MailController : Controller
    {
        //private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public MailController(/*IMapper mapper*/ IMailService mailService)
        {
            //_mapper = mapper;
            this._mailService = mailService;
        }



        /// <summary>
        /// 获取本用户的邮件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAsync(string type)
        {
            if (string.IsNullOrEmpty(type))
                return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync("")));
            if (type == "index")
                return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync("")));
            if (type == "draft")
                return await Task.FromResult(Json(await this._mailService.GetDraftMailAsync("")));
            if (type == "sended")
                return await Task.FromResult(Json(await this._mailService.GetSendedMailAsync("")));
            return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync("")));
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

        public async Task<IActionResult> GetMail(string mailId)
        {
            var item = await this._mailService.GetMailAsync(mailId);
            return Json(item);
        }
    }
}
