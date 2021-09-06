using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class CreateActivityViewModel
    {
        public string Name { get; set; }
        public string OrgId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mission { get; set; }
        public string Commission { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsActual { get; set; }
        public string Banner { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Requirement { get; set; }
        public string Description { get; set; }
        public string Targets { get; set; }
        public int AreaId { get; set; }

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
    }
}