using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IIdentityService
    {
        User FindUserById(string userId);
        User GetCurrentUser();

        bool IsLoggedIn();

        string GetCurrentUserId();

        User GetCurrentUserWithAddresses();
    }
}