using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.ResolveReportPage
{
    public partial class Filter : ComponentBase
    {
        private DateTime timeValue;
        private string valueStatus;
        private string valueTypeReport;
        private bool isFilterMonth;
        private bool haveStatus;
        private bool haveType;

        private FilterReportViewModel filter = new();

        [Parameter] public EventCallback<FilterReportViewModel> OnFilterChanged { get; set; }

        protected override void OnInitialized()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            valueStatus = "Trạng thái";
            valueTypeReport = "Loại báo cáo";
            isFilterMonth = false;
            haveStatus = false;
            haveType = false;
            timeValue = DateTime.Now;
        }

        public void ChooseValueType(bool isReportUser)
        {
            filter.IsReportUser = isReportUser;
            haveType = true;
        }

        public void ChooseValueStatus(ReportState state)
        {
            haveStatus = true;
            filter.State = state;
        }

        private void OnTimeValueChanged(DateTime time)
        {
            filter.Time = time;
            timeValue = time;
            isFilterMonth = true;
        }

        private async Task ClearFilterAsync()
        {
            SetDefaultValues();

            filter = new();

            await OnClickFilterAsync();
        }

        private async Task OnClickFilterAsync()
        {
            await OnFilterChanged.InvokeAsync(filter);
        }
    }
}
