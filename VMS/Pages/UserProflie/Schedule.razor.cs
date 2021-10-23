using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Common.Enums;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.UserProflie
{
    public partial class Schedule : ComponentBase
    {
        private List<ActivityViewModel> items = new();

        [Parameter] public bool IsUser { get; set; } = false;
        [Parameter] public string UserId { get; set; }

        [Inject] private IActivityService ActivityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await OnValueChanged(DateTime.Now);
        }

        private async Task OnValueChanged(DateTime value)
        {
            items = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Current, value);
        }
    }
}
