using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class InfoPage : ComponentBase
    {
        private User currentUser;
        private bool isFav;

        [Parameter]
        public ViewActivityViewModel Activity { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            currentUser = IdentityService.GetCurrentUserWithFavoritesAndRecruitments();
        }

        protected override void OnParametersSet()
        {
            if (currentUser is not null)
            {
                isFav = currentUser.Favorites.Any(f => f.ActivityId == Activity.Id);
            }
        }

        private void HandleFavorite(int id)
        {
            if (currentUser is null)
            {
                // TODO: Show login pop-up or edit info pop-up

                return;
            }

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
            isFav = !isFav;
        }
    }
}