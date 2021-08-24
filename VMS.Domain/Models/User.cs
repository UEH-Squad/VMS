using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using VMS.Domain.Interfaces;

namespace VMS.Domain.Models
{
    public class User : IdentityUser, IAuditEntity<string>, IDeleteEntity<string>
    {
        // For user
        public string StudentId { get; set; }

        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Avatar { get; set; }

        // For org + seeker
        public string PhoneNumber2 { get; set; }

        public string Website { get; set; }
        public string Mission { get; set; }
        public string Introduction { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
        public virtual ICollection<UserArea> UserAreas { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Recruitment> Recruitments { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Activity> ActivityApprovals { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}