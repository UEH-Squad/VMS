using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class FilterActivityViewModel
    {
        public bool Virtual { get; set; }
        public bool Actual { get; set; }
        public string OrgId { get; set; }
        public List<Area> Areas { get; set; }
        public List<Skill> Skills { get; set; }
        public List<AddressPath> AddressPaths { get; set; }

        public FilterActivityViewModel()
        {
            Virtual = true;
            Actual = false;
            OrgId = string.Empty;
            Areas = new List<Area>();
            Skills = new List<Skill>();
            AddressPaths = new List<AddressPath>();
        }
    }
}
