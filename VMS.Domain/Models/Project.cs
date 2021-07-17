using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Domain.Models
{
    public class Project : Entity
    {
        [ForeignKey(nameof(Organization))]
        public string OrganizationId { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Skills { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public string SpecialInstructions { get; set; }
        public int? MaxVolunteer { get; set; }
        public bool? IsApproved { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Category Category { get; set; }
    }
}