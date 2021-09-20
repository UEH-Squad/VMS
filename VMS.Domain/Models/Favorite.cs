using System;

namespace VMS.Domain.Models
{
    public class Favorite : EntityBase<int>
    {
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
