using System.Collections.Generic;
using VMS.Common.Enums;

namespace VMS.Application.ViewModels
{
    public class FilterRecruitmentViewModel
    {
        public string SearchValue { get; set; }
        public bool IsSearch { get; set; }

        public bool? IsRated { get; set; }

        public string OrgId { get; set; }
        public Semester Semester { get; set; }
        public bool IsOrgRating { get; set; } = false;
        public bool IsUserRating { get; set; } = false;
        public List<double> Ranks { get; set; } = new();
        public FilterRecruitmentViewModel()
        {
            Semester = Semester.Full;
        }
    }
}
