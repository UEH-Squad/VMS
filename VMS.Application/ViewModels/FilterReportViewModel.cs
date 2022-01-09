using System;
using VMS.Common.Enums;

namespace VMS.Application.ViewModels
{
    public class FilterReportViewModel
    {
        public bool IsReportUser { get; set; }
        public ReportState State { get; set; }
        public DateTime Time { get; set; }
    }
}
