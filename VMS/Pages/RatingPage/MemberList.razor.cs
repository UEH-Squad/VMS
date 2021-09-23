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

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            recruitments = await RecruitmentService.GetAllRecruitmentsAsync(ActivityId, page);
        }

        private async Task UpdateRatingAsync(double? rating, RecruitmentViewModel recruitment)
        {
            recruitment.Rating = rating;
            await RecruitmentService.UpdateRatingAsync(recruitment.Id, rating.Value);
        }
    }
}
