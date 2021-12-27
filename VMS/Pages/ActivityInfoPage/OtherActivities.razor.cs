using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class OtherActivities : ComponentBase
    {
        private List<ViewActivityViewModel> otherActivities;
        private User currentUser;

        [Parameter]
        public int ActivityId { get; set; }

        [Parameter]
        public string OrgId { get; set; }

        [CascadingParameter]
        public string CurrentUserId { get; set; }

        [Inject]
        public IJSRuntime JSRuntinme { get; set; }

        [Inject]
        public IActivityService ActivityService { get; set; }

        [Inject]
        public IIdentityService IdentityService { get; set; }
        
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            currentUser = IdentityService.GetUserWithFavoritesAndRecruitmentsById(CurrentUserId);
            otherActivities = await ActivityService.GetOtherActivitiesAsync(OrgId, new[] { ActivityId });
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && otherActivities.Count > 0)
            {
                await JSRuntinme.InvokeVoidAsync("vms.OtherAct");
            }
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

        private void NavigationTo(int activityId)
        {
            NavigationManager.NavigateTo(Routes.ActivityInfo + "/" + activityId, true);
        }
    }
}