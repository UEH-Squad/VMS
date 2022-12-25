using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.ActivityLogPage
{
    public partial class ActivityCard
    {
        private int page;
        private PaginatedList<RecruitmentViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public int StarRating { get; set; }
        [Parameter] public FilterRecruitmentViewModel FilterChanged { get; set; }

        [CascadingParameter] public IModalService ReportModal { get; set; }
        [CascadingParameter] public IModalService CommentModal { get; set; }
        [CascadingParameter] public string UserId { get; set; }

        [Inject] private IRecruitmentService RecruitmentService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            page = 1;
            pagedResult = await RecruitmentService.GetAllActivitiesAsync(FilterChanged, UserId, page);
        }

        private async Task HandlePageChangedAsync()
        {
            pagedResult = await RecruitmentService.GetAllActivitiesAsync(FilterChanged, UserId, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private async Task UpdateRatingAsync(double? rating, RecruitmentViewModel recruitment)
        {
            await RecruitmentService.UpdateRatingAndCommentAsync(recruitment.Activity.Id, rating.Value, string.Empty, recruitment.Id, false);
            recruitment.Rating = rating;
        }

        private void ShowReportPopUp(int activityId)
        {
            ModalParameters parameters = new();
            parameters.Add("ActivityId", activityId);

            ReportModal.Show<ActivityInfoPage.PopUpReport>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowCommentPopUpAsync(RecruitmentViewModel recruitment)
        {
            var parameters = new ModalParameters();
            parameters.Add("UserTop", recruitment.Organizer);
            parameters.Add("UserBottom", recruitment.User);
            parameters.Add("RecruitmentRatingTop", recruitment.RecruitmentRatings.Find(x => x.IsOrgRating));
            parameters.Add("RecruitmentRatingBottom", recruitment.RecruitmentRatings.Find(x => !x.IsOrgRating));
            parameters.Add("RecruitmentId", recruitment.Id);
            parameters.Add("ActivityId", recruitment.Activity.Id);
            parameters.Add("IsOrgRating", false);

            await CommentModal.Show<Organization.RatingPage.PopUpComment>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task HandleReceiveGift(string userId, int activityId)
        {
            await RecruitmentService.UpdateIsGiftAsync(userId, activityId);
        }
    }
}
