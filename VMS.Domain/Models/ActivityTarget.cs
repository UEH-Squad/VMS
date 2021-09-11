using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Domain.Models
{
    public class ActivityTarget
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public string Target { get; set; }

        public virtual Activity Activity { get; set; }
    }
}
