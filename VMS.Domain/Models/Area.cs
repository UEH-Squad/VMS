using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Area : DeleteEntity<int>
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<UserArea> UserAreas { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}