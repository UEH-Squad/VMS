using System;
using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class OrgViewModel
    {
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Mission { get; set; }
        public string Banner { get; set; }
        public int ActivitiesNum { get; set; }
        public ICollection<UserArea> UserAreas { get; set; }
        public int CreateDate { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}