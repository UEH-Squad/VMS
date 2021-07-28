namespace VMS.Domain.Models
{
    public class ActivitySkill : DeleteEntity<int>
    {
        public int ActivityId { get; set; }
        public int SkillId { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Skill Skill { get; set; }
    }
}