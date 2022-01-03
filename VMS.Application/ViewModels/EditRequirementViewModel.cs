using System;
using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class EditRequirementViewModel
    {
        public bool IsReport { get; set; }
        public bool IsReportUser { get; set; }  
        public string Content { get; set; }
        public string PartToFix { get; set; }
        public int ActivityId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<string> Images { get; set; }

    }
}
