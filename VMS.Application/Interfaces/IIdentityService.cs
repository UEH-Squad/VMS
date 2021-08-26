using System.Collections.Generic;
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

        User GetCurrentUserWithFavorites();

        List<Favorite> GetFavoritesOfCurrentUser();

        List<Favorite> UpdateFavoritesOfCurrentUser(int activityId);
    }
}