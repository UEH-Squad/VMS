using System;
using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public bool IsPin { get; set; }
        public bool IsClosed { get; set; }
        public bool IsDeleted { get; set; }
        public double Rating { get; set; }
        public bool IsMenu { get; set; }
        public bool IsFav { get; set; }
        public int Favorites { get; set; }
        public string Province { get; set; }

        public User Organizer { get; set; }
        public ICollection<ActivityAddress> ActivityAddresses { get; set; }
        public List<ActivitySkill> ActivitySkills { get; set; }
    }
}