﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeSync.Skype
{
    public interface IMeetingService
    {
        Task CreateOnlineMeetingAsync(string subject, string description);
    }
}