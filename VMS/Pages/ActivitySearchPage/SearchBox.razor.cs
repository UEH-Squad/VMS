using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SearchBox
    {
        [Parameter]
        public string SearchValue { get; set; }

        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }

        private void UpdateSearchValueAsync(ChangeEventArgs e)
        {
            SearchValue = (string)e.Value;
        }

        private async Task SearchAsync()
        {
            await SearchValueChanged.InvokeAsync(SearchValue);
        }

        private void ClearSearchBox()
        {
            SearchValue = string.Empty;
        }
    }
}
