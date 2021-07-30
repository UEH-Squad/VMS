namespace VMS.Domain.Models
{
    public class UserSkill : DeleteEntity<int>
    {
        public string UserId { get; set; }
        public int SkillId { get; set; }

        public virtual User User { get; set; }
        public virtual Skill Skill { get; set; }
    }
}