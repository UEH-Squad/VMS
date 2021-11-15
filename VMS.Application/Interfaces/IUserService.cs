using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserProfileViewModel> GetUserProfileViewModelAsync(string userId);

        Task<CreateOrgProfileViewModel> GetOrgProfileViewModelAsync(string userId);

        Task UpdateUserProfile(CreateUserProfileViewModel userProfileViewModel, string userId);

        Task UpdateOrgProfile(CreateOrgProfileViewModel orgProfileViewModel, string userId);

        UserViewModel GetUserViewModel(string userId);

        void UpdateUserAvatar(string userId, string avatar);
        Task<HashSet<DateTime>> GetActivityDaysAsync(string userId, DateTime startDate, DateTime endDate);
    }
}