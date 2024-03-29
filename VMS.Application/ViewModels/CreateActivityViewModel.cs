﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    public class CreateActivityViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string OrgId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(8).Date;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(15).Date;
        public DateTime OpenDate { get; set; } = DateTime.Now.Date;
        public DateTime CloseDate { get; set; } = DateTime.Now.AddDays(7).Date;

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

        public string AreaName { get; set; }
        public string AreaIcon { get; set; }

        [RequiredGreaterThanZero]
        public int ProvinceId { get; set; }

        public string Province { get; set; }

        [RequiredGreaterThanZero]
        public int DistrictId { get; set; }

        public string District { get; set; }

        public int WardId { get; set; }

        public string Ward { get; set; }
        public bool IsDelete { get; set; }
        public bool IsClosed { get; set; }

        [RequiredHasItems]
        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
    }
}