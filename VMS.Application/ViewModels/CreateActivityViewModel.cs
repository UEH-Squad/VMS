using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class CreateActivityViewModel
    {
        public string OrgId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mission { get; set; }
        public bool IsVirtual { get; set; }
        public string Website { get; set; }
        public string Banner { get; set; }
        public string FullAddress { get; set; }

        [Required]
        public string Requirement { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int AreaId { get; set; }
        [Required]
        public string Name { get; set; }

        [Range(1, 11371, ErrorMessage = "Invalid Address")]
        public int ProvinceId { get; set; }
        [Range(1, 11371, ErrorMessage = "Invalid Address")]
        public int DistrictId { get; set; }
        [Range(1, 11371, ErrorMessage = "Invalid Address")]
        public int WardId { get; set; }

        public ICollection<Skill> Skills { get; set; }
    }
}
