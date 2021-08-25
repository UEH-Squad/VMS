using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class OtherActivitiesViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityBanner { get; set; }
    }
}