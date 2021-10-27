using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Shared.Components
{
    public partial class Calendar : ComponentBase
    {
        private string Today { get; set; }

        private List<string> WeekLists { get; set; }

        private static HashSet<DateTime> hasActivity;

        [Parameter] public DateTime Value { get; set; }

        [Parameter] public EventCallback<DateTime> ValueChanged { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        [CascadingParameter]
        public string UserId { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            if (Value == DateTime.MinValue)
            {
                Value = DateTime.Today;
            }

            Today = "Hôm nay";
            WeekLists = new List<string>() { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };

            hasActivity = await UserService.GetActivityDaysAsync(UserId, StartDate, EndDate);
        }

        private DateTime EndDate => StartDate.AddDays(35);

        private DateTime StartDate
        {
            get
            {
                var date = Value.AddDays(1 - Value.Day);
                date = date.AddDays(0 - (int)date.DayOfWeek);
                return date;
            }
        }

        private EventCallback<MouseEventArgs> OnCellClickCallback(DateTime value)
            => EventCallback.Factory.Create<MouseEventArgs>(this, async () =>
        {
            Value = value;
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }

            hasActivity = await UserService.GetActivityDaysAsync(UserId, StartDate, EndDate);

            StateHasChanged();
        });

        private async Task OnChangeMonth(int offset)
        {
            Value = offset == 0 ? DateTime.Today : Value.AddMonths(offset);

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }

            hasActivity = await UserService.GetActivityDaysAsync(UserId, StartDate, EndDate);
        }

        private async Task OnValueChanged(DateTimeChangedEventArgs args)
        {
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }
}