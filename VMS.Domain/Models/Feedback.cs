using System;
using System.Collections.Generic;
using VMS.Domain.Interfaces;

namespace VMS.Domain.Models
{
    public class Feedback : EntityBase<int>, IAuditEntity
    {
        public bool IsRequest { get; set; }

        public string Content { get; set; }
        public bool IsReportUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int ActivityId { get; set; }
        public string UserId { get; set; }

        public bool IsPinned { get; set; }
        public bool? IsDone { get; set; }
        public bool IsClosed { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual User Handler { get; set; }
        public virtual User Reporter { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual ICollection<ReasonReport> ReasonReports { get; set; }
        public virtual ICollection<ImageReport> ImageReports { get; set; }
    }
}