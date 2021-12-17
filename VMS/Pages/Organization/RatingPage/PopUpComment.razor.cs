using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.Organization.RatingPage
{
    public partial class PopUpComment : ComponentBase
    {
        private bool isReadonly = false;
        private bool isEmpty = false;

        [Parameter] public UserViewModel UserBottom { get; set; }
        [Parameter] public UserViewModel UserTop { get; set; }
        [Parameter] public RecruitmentRatingViewModel RecruitmentRatingTop { get; set; }
        [Parameter] public RecruitmentRatingViewModel RecruitmentRatingBottom { get; set; }
        [Parameter] public int RecruitmentId { get; set; }
        [Parameter] public int ActivityId { get; set; }
        [Parameter] public bool IsOrgRating { get; set; } = true;

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance CommentModal { get; set; }

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

            ChangeState();
        }

        private async Task UpdateCommentAsync(EditContext editContext)
        {
            if (editContext.IsModified())
            {
                await RecruitmentService.UpdateRatingAndCommentAsync(ActivityId, null, RecruitmentRatingBottom.Comment, RecruitmentId, IsOrgRating);
            }

            ChangeState();
        }

        private void ChangeState()
        {
            isEmpty = string.IsNullOrEmpty(RecruitmentRatingBottom.Comment);
            isReadonly = !isEmpty && !isReadonly;
        }

        private async Task CloseModalAsync()
        {
            await CommentModal.CloseAsync();
        }
    }
}
