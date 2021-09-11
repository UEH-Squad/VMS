using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Domain.Models
{
    public class Activity : AuditEntity<int>
    {
        [ForeignKey(nameof(Organizer))]
        public string OrgId { get; set; }

        public int AreaId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Mission { get; set; }
        public string Requirement { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey(nameof(Approver))]
        public string ApprovedBy { get; set; }

        public bool IsVirtual { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Website { get; set; }
        public string Banner { get; set; }
        public bool IsPin { get; set; }

        public virtual User Organizer { get; set; }
        public virtual User Approver { get; set; }
        public virtual Area Area { get; set; }
        public virtual ICollection<ActivitySkill> ActivitySkills { get; set; }
        public virtual ICollection<ActivityAddress> ActivityAddresses { get; set; }
        public virtual ICollection<ActivityImage> ActivityImages { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}