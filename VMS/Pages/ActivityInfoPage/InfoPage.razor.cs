using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class InfoPage : ComponentBase
    {
        private User currentUser;
        private ViewActivityViewModel activity;

        [Parameter]
        public string ActivityId { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(int.Parse(ActivityId));
            currentUser = IdentityService.GetCurrentUserWithFavoritesAndRecruitments();
        }

        private void HandleFavorite(int id)
        {
            Favorite favorite = currentUser.Favorites.FirstOrDefault(f => f.ActivityId == id);

            if (favorite is null)
            {
                currentUser.Favorites.Add(new()
                {
                    UserId = currentUser.Id,
                    ActivityId = id,
                    CreatedDate = DateTime.Now
                });
            }
            else
            {
                currentUser.Favorites.Remove(favorite);
            }

            IdentityService.UpdateUser(currentUser);
        }
    }
}