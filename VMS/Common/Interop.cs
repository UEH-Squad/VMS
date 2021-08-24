using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace VMS.Common
{
    public class Interop
    {
        public static ValueTask ScrollToTop(IJSRuntime JsRuntime)
        {
            return JsRuntime.InvokeVoidAsync("window.scrollTo", 0, 0);
        }
    }
}