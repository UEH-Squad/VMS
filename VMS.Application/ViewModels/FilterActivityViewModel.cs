using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class FilterActivityViewModel
    {
        public bool Virtual { get; set; }
        public bool Actual { get; set; }
        public string OrgId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public List<int> Areas { get; set; }
        public List<int> Skills { get; set; }

        public int AddressPathId
        {
            get => ProvinceId != 0 ? (DistrictId != 0 ? DistrictId : ProvinceId) : 0;
        }

        public FilterActivityViewModel()
        {
            Virtual = true;
            Actual = false;
            Areas = new();
            Skills = new();
        }
    }
}