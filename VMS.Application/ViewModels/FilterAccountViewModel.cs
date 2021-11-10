using VMS.Common.Enums;

namespace VMS.Application.ViewModels
{
    public class FilterAccountViewModel
    {
        public string Role { get; set; }

        public string SearchValue { get; set; }
        public bool IsSearch { get; set; }

        public string Course { get; set; }
        public bool IsNewest { get; set; }
    }
}
