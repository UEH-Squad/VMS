using System;
using System.Collections.Generic;
using VMS.Domain.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsApproved { get; set; }

        public User Organizer { get; set; }
        public List<ActivitySkill> ActivitySkills { get; set; }

    }
}
