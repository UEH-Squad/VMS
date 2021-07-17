using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Domain.Models
{
    public class ProjectSkill : DeleteEntity<int>
    {
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Skill Skill { get; set; }
    }
}