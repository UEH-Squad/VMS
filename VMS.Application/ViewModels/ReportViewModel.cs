using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class ReportViewModel
    {
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

        public List<string> Reasons { get; set; }
        public List<string> Images { get; set; }
    }
}
