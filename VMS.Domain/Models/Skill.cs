using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Skill : DeleteEntity<int>
    {
        public int Name { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; }
        public virtual ICollection<ActivitySkill> ActivitySkills { get; set; }
    }
}