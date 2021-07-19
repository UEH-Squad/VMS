using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Domain.Models
{
    public class ProjectVolunteer : DeleteEntity<int>
    {
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(Volunteer))]
        public string VolunteerId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Volunteer Volunteer { get; set; }
    }
}