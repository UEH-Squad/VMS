using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Pages.RatingPage
{
    public partial class PopUpComment : ComponentBase
    {
        private double userRating;

        [Parameter] public User Organizer { get; set; }
        [Parameter] public User Member {  get; set; }
        [Parameter] public double OrgRating {  get; set; }
        [Parameter] public int RecruitmentId {  get; set; }

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userRating = await RecruitmentService.GetRatingByIdAsync(RecruitmentId);
        }
    }
}
