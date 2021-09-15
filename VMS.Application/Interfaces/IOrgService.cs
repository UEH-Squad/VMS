﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IOrgService
    {
        Task<CreateOrgProfileViewModel> GetCreateOrgProfileViewModelAsync(string orgId);
        Task UpdateOrgProfile(CreateOrgProfileViewModel orgProfileViewModel, string orgId);
    }
}
