using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Domain.Models
{
     public class ActivityAddress:DeleteEntity<int>
    {
        public int ActivityId { get; set; }
        public int AddressPathId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual AddressPath AddressPath { get; set; }
    }
}
