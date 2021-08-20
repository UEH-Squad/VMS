using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Skill : DeleteEntity<int>
    {
        public string Name { get; set; }
        public int? ParentSkillId { get; set; }

        public virtual Skill ParentSkill { get; set; }
        public virtual ICollection<Skill> SubSkills { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
        public virtual ICollection<ActivitySkill> ActivitySkills { get; set; }
    }
}