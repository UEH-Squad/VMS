using System;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PostDate { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }

        public User Organizer { get; set; }

    }
}
