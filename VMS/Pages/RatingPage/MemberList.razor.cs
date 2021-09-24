using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Pages.RatingPage
{
    public partial class MemberList : ComponentBase
    {
        private PaginatedList<RecruitmentViewModel> recruitments = new(new(), 0, 1, 1);

        [Parameter] public int ActivityId { get; set; }
        [Parameter] public bool? IsRated { get; set; }
        [Parameter] public string SearchValue { get; set; }

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
    }
}
