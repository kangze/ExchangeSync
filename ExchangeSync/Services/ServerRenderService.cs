﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExchangeSync.Services
{
    public class ServerRenderService : IServerRenderService
    {
        private readonly string _serverFileName;

        public ServerRenderService(string serverFileName)
        {
            _serverFileName = serverFileName;
        }

        public string Render(string path, object data, object user)
        {
            var sb = new StringBuilder();
            var dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(new { data, user });
            var base64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataStr));
            var psi = new ProcessStartInfo("node", this._serverFileName + " " + path + " " + base64Str)
            {
                RedirectStandardOutput = true,
            };
            //psi.StandardInputEncoding = Encoding.UTF8;
            psi.StandardOutputEncoding = Encoding.UTF8;
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
                throw new Exception("proccess has not start!");
            //开始读取
            try
            {
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                        sb.AppendLine(sr.ReadLine());
                    if (!proc.HasExited) proc.Kill();
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                if (!proc.HasExited) proc.Kill();
                throw;
            }


        }
    }
}
