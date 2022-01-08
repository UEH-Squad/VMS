using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class RecruitmentRating : DeleteEntity<int>
    {
        public int RecruitmentId { get; set; }
        public string Comment { get; set; }
        public double Rank { get; set; }
        public string ReportContent { get; set; }
        public bool IsOrgRating { get; set; }

        public virtual Recruitment Recruitment { get; set; }
    }
}