using System;
using System.Collections.Generic;
using VMS.Common.Enums;

namespace VMS.Application.ViewModels
{
    public class FilterActivityViewModel
    {
        public string SearchValue { get; set; }
        public bool IsSearch { get; set; }

        public bool Virtual { get; set; }
        public bool Actual { get; set; }
        public bool Covid { get; set; }
        public string OrgId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Level { get; set; }
        public StatusAct ActType { get; set; } = StatusAct.All;
        public bool IsMonthFilter { get; set; } = false;
        public DateTime DateTimeValue { get; set; }
        public bool? IsApproved { get; set; }
        public List<AreaViewModel> Areas { get; set; }
        public List<SkillViewModel> Skills { get; set; }

        public int AddressPathId
        {
            get => WardId != 0 ? WardId : DistrictId != 0 ? DistrictId : ProvinceId;
        }

        public FilterActivityViewModel()
        {
            Virtual = false;
            Actual = false;
            Covid = false;
            Areas = new();
            Skills = new();
        }
    }
}