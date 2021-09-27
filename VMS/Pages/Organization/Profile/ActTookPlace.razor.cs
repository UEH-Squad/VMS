using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using System.Collections.Generic;

namespace VMS.Pages.Organization.Profile
{
    public partial class ActTookPlace : ComponentBase
    {
        [Parameter]
        public List<ActivityViewModel> ActEnded { get; set; }

        private static bool HaftStar(double rate, int star)
        {
            if (rate - star > 0 && rate - star <= 0.5)
            {
                return true;
            }
            return false;
        }
    }

}