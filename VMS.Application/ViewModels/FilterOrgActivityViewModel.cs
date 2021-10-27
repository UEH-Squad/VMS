namespace VMS.Application.ViewModels
{
    public class FilterOrgActivityViewModel
    {
        public bool IsSearch { get; set; }
        public string SearchValue { get; set; }

        public string OrgId { get; set; }
        public bool IsHappenning { get; set; }
        public bool IsTookPlace { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsActual { get; set; }
    }
}
