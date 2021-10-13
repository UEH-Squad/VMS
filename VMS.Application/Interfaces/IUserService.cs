using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserProfileViewModel> GetCreateUserProfileViewModelAsync(string userId);
        Task UpdateUserProfile(CreateUserProfileViewModel userProfileViewModel, string userId);
    }
}
