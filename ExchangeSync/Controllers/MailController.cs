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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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

        public async Task<IActionResult> GetMail(string mailId)
        {

            var item = await this._mailService.GetMailAsync(mailId);
            return Json(item);
        }

        public async Task<IActionResult> SetUnReade(string mailId)
        {
            await this._mailService.SetUnReade(mailId);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Delete(string mailId)
        {
            await this._mailService.Delete(mailId);
            return Json(new { success = true });
        }

        public async Task<IActionResult> DownloadAttachment(string mailId, string attachmentId, string attachmentName)
        {
            var stream = await this._mailService.Download(mailId, attachmentId);
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

        public async Task<IActionResult> Reply([FromForm]ReplyMailInput input)
        {
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
                await this._mailService.ReplyAsync(input.MailId, input.Content, input.CopyTo, input.Attachments);
                return Json(new { success = true });
            }
            else
            {
                await this._mailService.Send(input.Title, input.Content, input.Reciver, input.CopyTo, input.Attachments);
                return Json(new { success = true });
            }

        }
    }
}
