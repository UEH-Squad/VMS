using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace VMS.Common
{
    public class Interop
    {
        public static ValueTask ScrollToTop(IJSRuntime JsRuntime)
        {
            return JsRuntime.InvokeVoidAsync("vms.SmoothScrollTo", "html, body");
        }
    }
}