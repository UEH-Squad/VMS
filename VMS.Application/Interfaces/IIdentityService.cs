using Microsoft.AspNetCore.Identity;

namespace VMS.Application.Interfaces
{
    public interface IIdentityService
    {
        IdentityUser FindUserById(string userId);
        IdentityUser GetCurrentUser();
    }
}