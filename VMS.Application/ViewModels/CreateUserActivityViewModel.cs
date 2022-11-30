using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class CreateUserActivityViewModel
    {
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public bool IsGift { get; set; }
        public DateTime CreateDate { get; set; }
        //public List<string> Images { get; set; }
        //public List<string> Reasons { get; set; }
    }
}
