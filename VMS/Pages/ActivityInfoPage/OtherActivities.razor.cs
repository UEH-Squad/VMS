using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class OtherActivities : ComponentBase
    {
        private ViewActivityViewModel activity;
        private List<OtherActivitiesViewModel> otherActivities;
        private User currentUser;

        [Parameter]
        public string ActivityId { get; set; }

        [Parameter]
        public string OrgId { get; set; }

        [Inject]
        public IJSRuntime JSRuntinme { get; set; }

        [Inject]
        public IActivityService ActivityService { get; set; }

        [Inject]
        public IIdentityService IdentityService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (otherActivities is not null)
            {
                await JSRuntinme.InvokeVoidAsync("vms.OtherAct", otherActivities.Count);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(int.Parse(ActivityId));
            OrgId = activity.OrgId;
            otherActivities = await ActivityService.GetOtherActivities(OrgId);
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