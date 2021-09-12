using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.Services;

namespace VMS.Pages.Organization.Profile
{
    public partial class Information : ComponentBase
    {
        private OrgViewModel org;
        [Parameter]
        public string OrgId { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            org = await OrganizationService.GetOrgAsync(OrgId);
        }
    }

}