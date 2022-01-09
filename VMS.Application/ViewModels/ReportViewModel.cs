using System;
using System.Collections.Generic;
using System.Linq;

namespace VMS.Application.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public string ReportBy { get; set; }
        public bool IsReportUser { get; set; }

        public bool IsRequest { get; set; }

        public bool IsPinned { get; set; }
        public bool? IsDone { get; set; }
        public bool IsClosed { get; set; }

        public string ActivityName { get; set; }
        public string HandlerName { get; set; }
        public string ReporterName { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<string> Reasons { get; set; }
        public List<string> Images { get; set; }

        public ReportViewModel()
        {
            CreatedDate = DateTime.Now;
            Reasons = new();
            Images = new();
        }
    }
}
