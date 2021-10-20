using Blazored.Modal;
using Blazored.Modal.Services;
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
using VMS.GenericRepository;

namespace VMS.Pages.ActivityLogPage
{
    public partial class ActivityCard
    {
        private int page = 1;
        private string userId;
        private PaginatedList<RecruitmentViewModel> pagedResult = new(new(), 0, 1, 1);

        [CascadingParameter]
        public IModalService ReportModal { get; set; }

        [CascadingParameter]
        public IModalService CommentModal { get; set; }

        [Parameter] public int StarRating { get; set; }
        [Parameter] public bool? IsRated { get; set; }
        [Parameter] public string SearchValue { get; set; }

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userId = IdentityService.GetCurrentUserId();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (StarRating != 0)
            {
                await UpdateRatingAsync(StarRating);
                StarRating = 0;
            }

            pagedResult = await RecruitmentService.GetAllActivitiesAsync(userId, page, SearchValue, IsRated);
        }

        private async Task HandlePageChanged()
        {
            pagedResult = await RecruitmentService.GetAllActivitiesAsync(userId, page, SearchValue, IsRated);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private async Task UpdateRatingAsync(double? rating, RecruitmentViewModel recruitment = null)
        {
            await RecruitmentService.UpdateRatingAndCommentAsync(rating.Value, string.Empty, recruitment?.Id);
            if (recruitment != null)
            {
                recruitment.Rating = rating;
            }
        }

        private async Task ShowReportPopUp()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            ReportModal.Show<ActivityInfoPage.PopUpReport>("", options);
        }

        private async Task ShowCommentPopUpAsync(RecruitmentViewModel recruitment)
        {
            var parameters = new ModalParameters();
            parameters.Add("UserTop", recruitment.Activity.Organizer);
            parameters.Add("UserBottom", recruitment.User);
            parameters.Add("RecruitmentRatingTop", recruitment.RecruitmentRatings.Find(x => !x.IsOrgRating));
            parameters.Add("RecruitmentRatingBottom", recruitment.RecruitmentRatings.Find(x => x.IsOrgRating));
            parameters.Add("RecruitmentId", recruitment.Id);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await CommentModal.Show<Organization.RatingPage.PopUpComment>("", parameters, options).Result;
        }

    }
}
