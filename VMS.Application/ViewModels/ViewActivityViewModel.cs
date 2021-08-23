using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class ViewActivityViewModel
    {
        public string OrgId { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Mission { get; set; }
        public string Requirement { get; set; }
        public bool IsVirtual { get; set; }
        public string Website { get; set; }
        public string Banner { get; set; }
        public bool IsApproved { get; set; }

        public Area Area { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<ActivityImage> ActivityImages { get; set; }
    }
}