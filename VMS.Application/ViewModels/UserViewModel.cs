using System;
using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Mission { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string NotifiedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public double AverageRating { get; set; }
        public double QuantityRating { get; set; }
        public double Rank { get; set; }
        public string Introduction { get; set; }
        public string Banner { get; set; }
        public string StudentId { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }

        public string Faculty { get; set; }

        public int TotalActivitiesDay { get; set; }
        public int TotalArea { get; set; }
        public int TotalOrganisation { get; set; }

        public List<AreaViewModel> Areas { get; set; }
        public List<Activity> Activities { get; set; }
        public List<SkillViewModel> Skills {  get; set; }

        public UserViewModel()
        {
            Areas = new();
            Activities = new();
            Skills = new();
        }
    }
}