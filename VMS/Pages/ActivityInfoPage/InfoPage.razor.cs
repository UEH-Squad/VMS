using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class InfoPage : ComponentBase
    {
        private User currentUser;
        private bool isFav;

        [Parameter]
        public ViewActivityViewModel Activity { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        [CascadingParameter] public string CurrentUserId { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            currentUser = IdentityService.GetUserWithFavoritesAndRecruitmentsById(CurrentUserId);
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
                Modal.Show<Shared.Components.RequireSignup>("", BlazoredModalOptions.GetModalOptions());

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

        private async Task ShowEditRequirement()
        {
            Modal.Show<Admin.ActivityManagement.EditRequirement>("", BlazoredModalOptions.GetModalOptions());
        }

        private void OnClickNavigateToEditActivty(int activityId)
        {
            NavigationManager.NavigateTo(Routes.EditActivity + "/" + activityId, true);
        }
    }    
}