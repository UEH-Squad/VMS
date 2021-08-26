using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Domain.Models
{
    public class ReasonReport
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int ActivityId { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }

    }
}
