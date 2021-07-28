using System;
using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Recruitment : AuditEntity<int>
    {
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public DateTime EnrollTime { get; set; }
        public DateTime AcceptTime { get; set; }

        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }
        public ICollection<RecruitmentRating> RecruitmentRatings { get; set; }
    }
}