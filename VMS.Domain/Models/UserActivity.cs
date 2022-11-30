using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Domain.Models
{
    public class UserActivity : DeleteEntity<int>
    {
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public bool IsGift { get; set; }
        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
