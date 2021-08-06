using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class ActivityAddressViewModel
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int AddressPathId { get; set; }
    }
}
