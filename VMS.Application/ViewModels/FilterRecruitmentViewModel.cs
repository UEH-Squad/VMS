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

        public bool? IsOrgRating { get; set; }
        public List<double> Ranks { get; set; }

        public FilterRecruitmentViewModel()
        {
            Semester = Semester.Full;
            Ranks = new();
        }
    }
}
