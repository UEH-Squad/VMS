using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class ReportViewModel
    {
        public string DesReport { get; set; }
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public string ReportBy { get; set; }
        public bool IsReportUser { get; set; }

        public bool IsRequest { get; set; }

        public List<string> Reasons { get; set; }
        public List<string> Images { get; set; }
    }
}
