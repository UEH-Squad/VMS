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
    }
}