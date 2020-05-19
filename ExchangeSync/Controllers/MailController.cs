using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExchangeSync.Controllers
{
    public class MailController : ExchangeControllerBase
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
        [Authorize]
        public async Task<IActionResult> GetAsync(string type)
        {
            var userName = this.GetUserName();
            if (string.IsNullOrEmpty(type))
                return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync(userName)));
            if (type == "index")
                return Json(await this._mailService.GetIndexMailAsync(userName));
            if (type == "draft")
                return await Task.FromResult(Json(await this._mailService.GetDraftMailAsync(userName)));
            if (type == "sended")
                return await Task.FromResult(Json(await this._mailService.GetSendedMailAsync(userName)));
            return await Task.FromResult(Json(await this._mailService.GetIndexMailAsync(userName)));
        }

        //[Authorize]
        public async Task<IActionResult> GetMail(string mailId)
        {
            var userName = this.GetUserName();
            var item = await this._mailService.GetMailAsync(userName, mailId);
            return Json(item);
        }

        [Authorize]
        public async Task<IActionResult> GetContent(string mailId)
        {
            var userName = this.GetUserName();
            var item = await this._mailService.GetMailAsync(userName, mailId);
            return Content(item.Content, "text/html");
        }

        [Authorize]
        public async Task<IActionResult> SetUnReade(string mailId)
        {
            var userName = this.GetUserName();
            await this._mailService.SetUnReade(userName, mailId);
            return Json(new { success = true });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string mailId)
        {
            var userName = this.GetUserName();
            await this._mailService.Delete(userName, mailId);
            return Json(new { success = true });
        }

        //[Authorize]
        public async Task<IActionResult> DownloadAttachment(string mailId, string attachmentId, string attachmentName, string userName)
        {
            var _userName = userName;
            if (string.IsNullOrEmpty(_userName))
                _userName = this.GetUserName(); ;
            var stream = await this._mailService.Download(_userName, mailId, attachmentId);
            stream.Position = 0;
            return File(stream, "application/octet-stream", attachmentName);
        }

        private void ConvertImage(ReplyMailInput input)
        {
            //如果是图片的话,必须使用CID
            if (string.IsNullOrEmpty(input.Content))
                input.Content = "";
            if (input.Attachments == null)
                input.Attachments = new List<AttachmentInput>();
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            var matches = regImg.Matches(input.Content);
            if (matches.Count == 0)
                return;
            foreach (Match match in matches)
            {
                var imgSrc = match.Groups[1].Value;
                var imgBase64 = imgSrc.Split(',')[1];
                var imgBytes = Convert.FromBase64String(imgBase64);
                var attachmentId = Guid.NewGuid().ToString("N").ToLower() + ".jpg";
                input.Content = input.Content.Replace(imgSrc, "cid:" + attachmentId);

                input.Attachments.Add(new AttachmentInput()
                {
                    Id = attachmentId,
                    Name = attachmentId,
                    Bytes = imgBytes
                });
            }
        }

        [Authorize]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        public async Task<IActionResult> Reply([FromForm]ReplyMailInput input)
        {
            var userName = this.GetUserName();
            this.ConvertImage(input);
            if (input.Attachment != null)
            {
                foreach (var it in input.Attachment)
                {
                    var stream = it.OpenReadStream();
                    var bytes = new byte[it.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    var name = Guid.NewGuid().ToString("N").ToLower() + "-" + it.FileName;
                    input.Attachments.Add(new AttachmentInput()
                    {
                        Bytes = bytes,
                        Name = name,
                        Id = name,
                        IsPackage = true,
                    });
                }
            }
            if (input.CopyTo == null)
                input.CopyTo = new string[0];
            if (input.Attachments == null)
                input.Attachments = new List<AttachmentInput>();
            if (!string.IsNullOrEmpty(input.MailId))
            {
                await this._mailService.ReplyAsync(userName, input.MailId, input.Content, input.CopyTo, input.Attachments);
                return Json(new { success = true });
            }
            else
            {
                await this._mailService.Send(userName, input.Title, input.Content, input.Reciver, input.CopyTo, input.Attachments);
                return Json(new { success = true });
            }

        }
    }
}
