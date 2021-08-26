using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Signup : ComponentBase
    {
        private SignUpActivityViewModel information = new();

        [Required]
        private RadioOptions choosenRadio;

        [Parameter]
        public int ActivityId { get; set; }
        [Parameter]
        public User CurrentUser { get; set; }
        [CascadingParameter]
        private BlazoredModalInstance Modal { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        private enum RadioOptions { Commit, TakeTime }

        private async Task ExitModalAsync()
        {
            await Modal.CloseAsync();
        }

        private bool isShowReport = false;

        private async Task ShowReportSuccessAsync()
        {
            isShowReport = !isShowReport;

            HandleSignUp();

            if (isShowReport == false)
            {
                await Modal.CloseAsync();
            }
        }

        private void HandleSignUp()
        {
            if (!CurrentUser.Recruitments.Any(r => r.ActivityId == ActivityId))
            {
                CurrentUser.Recruitments.Add(new Recruitment()
                {
                    UserId = CurrentUser.Id,
                    ActivityId = ActivityId,
                    PhoneNumber = information.PhoneNumber,
                    Desire = information.Desire,
                    IsCommit = (choosenRadio == RadioOptions.Commit),
                    EnrollTime = System.DateTime.Now,
                    CreatedBy = CurrentUser.Id,
                    CreatedDate = System.DateTime.Now
                });
                IdentityService.UpdateUser(CurrentUser);
            }
        }
    }
}
