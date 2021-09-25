using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.RatingPage
{
    public partial class PopUpComment : ComponentBase
    {
        [Parameter] public User UserBottom { get; set; }
        [Parameter] public User UserTop {  get; set; }
        [Parameter] public RecruitmentRatingViewModel RecruitmentRatingTop { get; set; }
        [Parameter] public RecruitmentRatingViewModel RecruitmentRatingBottom { get; set; }
        [Parameter] public int RecruitmentId {  get; set; }

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }

        protected override void OnParametersSet()
        {
            if (RecruitmentRatingTop is null)
            {
                RecruitmentRatingTop = new();
            }

            if (RecruitmentRatingBottom is null)
            {
                RecruitmentRatingBottom = new();
            }
        }
    }
}
