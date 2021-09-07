using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    public class CreateActivityViewModel
    {
        [Required]
        public string Name { get; set; }

        public string OrgId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string Mission { get; set; }

        [Required]
        public string Commission { get; set; }

        public bool IsVirtual { get; set; }
        public bool IsActual { get; set; }

        public string Banner { get; set; }

        public string Address { get; set; }
        public string FullAddress { get; set; }

        [Required]
        public string Requirement { get; set; }

        [Required]
        public string Description { get; set; }

        public string Targets { get; set; }

        [RequiredGreaterThanZero]
        public int AreaId { get; set; }

        [RequiredGreaterThanZero]
        public int ProvinceId { get; set; }

        [RequiredGreaterThanZero]
        public int DistrictId { get; set; }

        [RequiredGreaterThanZero]
        public int WardId { get; set; }

        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
    }
}