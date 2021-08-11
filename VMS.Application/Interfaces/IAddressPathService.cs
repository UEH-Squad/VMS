﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IAddressPathService
    {
        Task InitializeAddressPathsAsync();
        string ToTitleCase(string str);
    }
}
