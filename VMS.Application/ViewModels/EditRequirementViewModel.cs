using System;
using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class EditRequirementViewModel
    {
        public int ActivityId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public List<string> Images { get; set; }
        public List<string> Reasons { get; set; }
    }
}
