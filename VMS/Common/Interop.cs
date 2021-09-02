using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace VMS.Common
{
    public static class Interop
    {
        public static ValueTask ScrollToTop(IJSRuntime JsRuntime)
        {
            return JsRuntime.InvokeVoidAsync("vms.SmoothScrollTo", "html, body");
        }

        internal static ValueTask<object> Focus(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<object>("blazoredTypeahead.setFocus", element);
        }

        internal static ValueTask<object> AddKeyDownEventListener(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<object>("blazoredTypeahead.addKeyDownEventListener", element);
        }

        internal static ValueTask<object> OnOutsideClick(this IJSRuntime jsRuntime, ElementReference element, object caller, string methodName, bool clearOnFire = false)
        {
            return jsRuntime.InvokeAsync<object>("blazoredTypeahead.onOutsideClick", element, DotNetObjectReference.Create(caller), methodName, clearOnFire);
        }
    }
}