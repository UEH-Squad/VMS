using System;
using System.Collections.Generic;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace VMS.Pages.UserProflie
{
    public partial class Schedule : ComponentBase
    {
        [Parameter] public bool IsUser { get; set; } = true;
        [Parameter] public EventCallback<DateTime> ScheduleDateTimeChanged { get; set; }
        [Parameter] public List<ActivityViewModel> Items {  get; set; }

        private async Task OnValueChanged(DateTime value)
        {
            await ScheduleDateTimeChanged.InvokeAsync(value);
        }
    }
}
