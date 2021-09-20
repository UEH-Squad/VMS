using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class SearchBar : ComponentBase
    {
        [Parameter]
        public string SearchValue { get; set; }

        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }

        private void UpdateSearchValue(ChangeEventArgs e)
        {
            SearchValue = (string)e.Value;
        }

        private async Task OnKeyDownAsync(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await SearchAsync();
            }
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
