using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Pages.Organization.RatingPage
{
    public partial class MemberList : ComponentBase
    {
        private int page = 1;
        private PaginatedList<RecruitmentViewModel> recruitments = new(new(), 0, 1, 1);

        [Parameter] public int ActivityId { get; set; }
        [Parameter] public bool? IsRated { get; set; }
        [Parameter] public string SearchValue { get; set; }
        [Parameter] public UserViewModel Organizer { get; set; }
        [Parameter] public int StarRating { get; set; }


        [CascadingParameter]
        public IModalService CommentModal { get; set; }
        [CascadingParameter]
        public IModalService ReportModal { get; set; }

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (StarRating != 0)
            {
                await UpdateRatingAsync(StarRating);
                StarRating = 0;
            }

            recruitments = await RecruitmentService.GetAllRecruitmentsAsync(ActivityId, 1, SearchValue, IsRated);
        }

        private async Task HandlePageChangedAsync()
        {
            recruitments = await RecruitmentService.GetAllRecruitmentsAsync(ActivityId, page, SearchValue, IsRated);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private async Task UpdateRatingAsync(double? rating, RecruitmentViewModel recruitment = null)
        {
            await RecruitmentService.UpdateRatingAndCommentAsync(ActivityId, rating.Value, string.Empty, recruitment?.Id);
            if (recruitment != null)
            {
                recruitment.Rating = rating;
            }
        }

        private async Task ShowCommentPopUpAsync(RecruitmentViewModel recruitment)
        {
            var parameters = new ModalParameters();
            parameters.Add("UserBottom", Organizer);
            parameters.Add("UserTop", recruitment.User);
            parameters.Add("RecruitmentRatingTop", recruitment.RecruitmentRatings.Find(x => !x.IsOrgRating));
            parameters.Add("RecruitmentRatingBottom", recruitment.RecruitmentRatings.Find(x => x.IsOrgRating));
            parameters.Add("RecruitmentId", recruitment.Id);
            parameters.Add("ActivityId", ActivityId);

            await CommentModal.Show<PopUpComment>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private void ShowReportPopUp(string UserId)
        {
            List<string> reasons = new()
            {
                "Đây là một tài khoản giả mạo",
                "Đánh giá người dùng không tham gia hoạt động",
                "Khác"
            };

            var parameters = new ModalParameters();
            parameters.Add("ActivityId", ActivityId);
            parameters.Add("UserId", UserId);
            parameters.Add("Reasons", reasons);
            parameters.Add("IsReportUser", true);

            CommentModal.Show<ActivityInfoPage.PopUpReport>("", parameters, BlazoredModalOptions.GetModalOptions());
        }
    }
}
