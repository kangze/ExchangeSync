﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Exchange;
using ExchangeSync.Exchange.Internal;
using ExchangeSync.Helper;
using ExchangeSync.Model.ExchangeModel;
using ExchangeSync.Models;
using ExchangeSync.Models.Inputs;
using ExchangeSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSync.Controllers
{
    public class MailController : Controller
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            this._mailService = mailService;
        }



        /// <summary>
        /// 获取本用户的邮件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAsync(string type)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            if (string.IsNullOrEmpty(type))
                return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync("")));
            if (type == "index")
                return Json(await this._mailService.GetIndexMailAsync(""));

            if (type == "draft")
                return await Task.FromResult(Json(await this._mailService.GetDraftMailAsync("")));
            if (type == "sended")
                return await Task.FromResult(Json(await this._mailService.GetSendedMailAsync("")));
            return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync("")));
        }

        /// <summary>
        /// 获取邮件
        /// </summary>
        /// <returns></returns>
        private async Task<List<MailInfo>> GetInbox()
        {
            var mailManager = MailManager.Create("v-ms-kz@scrbg.com", "tfs4418000");
            var result = await mailManager.GetInBoxMessageAsync();
            return result;
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
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            var item = await this._mailService.GetMailAsync(mailId);
            return Json(item);
        }

        public async Task<IActionResult> DownloadAttachment(string mailId, string attachmentId, string attachmentName)
        {
            var stream = await this._mailService.Download(mailId, attachmentId);
            stream.Position = 0;
            return File(stream, "application/octet-stream", attachmentName);
        }

        private string ConvertImage(string content)
        {
            //如果是图片的话,必须使用CID
            //if (string.IsNullOrEmpty(content))
            //    content = "";
            //Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //var matches = regImg.Matches(content);
            //if (matches.Count == 0)
            //    return content;
            //foreach (Match match in matches)
            //{
            //    var imgBase64 = match.Groups[1].Value;
            //    var id = Guid.NewGuid().ToString("N");
            //    ImagesController.Images.Add(id, imgBase64);
            //    content=content.Replace(imgBase64, Request.Scheme+"://"+Request.Host+ "/img/1.jpg");
            //}
            return content;
        }

        public async Task<IActionResult> Reply([FromBody]ReplyMailInput input)
        {
            input.Content = ConvertImage(input.Content);
            await this._mailService.ReplyAsync(input.MailId, input.Content);
            if (!string.IsNullOrEmpty(input.MailId))
            {
                return Json(new { success = true });
            }
            else
            {
                foreach (var item in input.Reciver)
                {
                    await this._mailService.Send(input.Title, input.Content, item);
                }
                return Json(new { success = true });
            }

        }
    }
}
