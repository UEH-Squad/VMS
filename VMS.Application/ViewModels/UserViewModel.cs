using System;
using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class UserViewModel
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Mission { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserArea> UserAreas { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public float AverageRating { get; set; }
       public double QuantityRating { get; set; }
       public double Rank { get; set; }
        public string Introduction { get; set; }
        public string Banner { get; set; }
    }
}