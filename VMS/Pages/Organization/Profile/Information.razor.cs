using System.Threading.Tasks;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Information : ComponentBase
    {
        private OrgRatingViewModel org;
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        protected override void OnInitialized()
        {
            org = OrganizationService.GetOrgRating(UserId);
        }
    }

}