using System.Collections.Generic;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IIdentityService
    {
        User FindUserById(string userId);

        User GetCurrentUser();

        bool IsLoggedIn();

        string GetCurrentUserId();

        List<User> GetAllOrganizers();

        string GetCurrentUserAddress();

        User GetCurrentUserWithAddresses();

        User GetCurrentUserWithFavoritesAndRecruitments();

        void UpdateUser(User user);

        bool IsCorrectCurrentUserPassword(string password);
    }
}