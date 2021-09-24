using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Pages.RatingPage
{
    public partial class MemberList : ComponentBase
    {
        private PaginatedList<RecruitmentViewModel> recruitments = new(new(), 0, 1, 1);

        [Parameter] public int ActivityId { get; set; }
        [Parameter] public bool? IsRated { get; set; }
        [Parameter] public string SearchValue { get; set; }
        [Parameter] public User Organizer { get; set; }


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
            recruitments = await RecruitmentService.GetAllRecruitmentsAsync(ActivityId, page, SearchValue, IsRated);
        }

        private async Task UpdateRatingAsync(double? rating, RecruitmentViewModel recruitment = null)
        {
            await RecruitmentService.UpdateRatingAsync(rating.Value, recruitment?.Id);
            if (recruitment != null)
            {
                recruitment.Rating = rating;
            }
        }


        private async Task ShowCommentPopUpAsync(RecruitmentViewModel recruitment)
        {
            var parameters = new ModalParameters();
            parameters.Add("Organizer", Organizer);
            parameters.Add("Member", recruitment.User);
            parameters.Add("OrgRating", recruitment.Rating);
            parameters.Add("RecruitmentId", recruitment.Id);

            var options = new ModalOptions()
            {

                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await CommentModal.Show<PopUpComment>("", parameters, options).Result;
        }
    }
}
