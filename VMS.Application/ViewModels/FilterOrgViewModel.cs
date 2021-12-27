using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class FilterOrgViewModel
    {
        public string SearchValue { get; set; }
        public bool IsSearch { get; set; }

        public string Course { get; set; }
        public List<AreaViewModel> Areas { get; set; }

        public FilterOrgViewModel()
        {
            Areas = new();
        }
    }
}
