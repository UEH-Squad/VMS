using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SearchBox
    {
        [Parameter]
        public string SearchValue { get; set; }

        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }
        [Parameter] public bool IsUsedForAdmin { get; set; }


        private void UpdateSearchValue(ChangeEventArgs e)
        {
            SearchValue = (string)e.Value;
        }

        private async Task OnKeyUpAsync(KeyboardEventArgs e)
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
