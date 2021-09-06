using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }
    }
}
