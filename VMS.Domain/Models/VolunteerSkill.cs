using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Domain.Models
{
    public class VolunteerSkill : Entity
    {
        [ForeignKey(nameof(Volunteer))]
        public string VolunteerId { get; set; }

        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }

        public virtual Volunteer Volunteer { get; set; }
        public virtual Skill Skill { get; set; }
    }
}