﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Services.Dtos;

namespace ExchangeSync.Services
{
    public interface IOaSystemOperationService
    {
        Task<bool> CreateAppointmentAsync(OAAppoinmentInputDto input);
    }
}
