using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace VMS.Common
{
    public static class Interop
    {
        internal static ValueTask ScrollToTop(IJSRuntime JsRuntime)
        {
            return JsRuntime.InvokeVoidAsync("vms.SmoothScrollTo", "html, body");
        }

        internal static ValueTask<object> Focus(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<object>("typeahead.setFocus", element);
        }

        internal static ValueTask<object> AddKeyDownEventListener(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<object>("typeahead.addKeyDownEventListener", element);
        }

        internal static ValueTask<object> OnOutsideClick(this IJSRuntime jsRuntime, ElementReference element, object caller, string methodName, bool clearOnFire = false)
        {
            return jsRuntime.InvokeAsync<object>("typeahead.onOutsideClick", element, DotNetObjectReference.Create(caller), methodName, clearOnFire);
        }
    }
}