using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    public class CreateUserProfileViewModel
    {
        [Required] public string FullName { get; set; }
        [Required] public string NotifiedEmail { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Mission { get; set; }
        [Required] public string UserName { get; set; }


        [RequiredHasItems]
        public IList<AreaViewModel> Areas { get; set; } = new List<AreaViewModel>();
        [RequiredHasItems]
        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();


        [RequiredGreaterThanZero]
        public int? FacultyId { get; set; }


        [RequiredGreaterThanZero]
        public int ProvinceId { get; set; }
        public string Province { get; set; }

        [RequiredGreaterThanZero]
        public int DistrictId { get; set; }
        public string District { get; set; }

        public int WardId { get; set; }
        public string Ward { get; set; }

        public string Address { get; set; }
        public string FullAddress { get; set; }

        public DateTime Birthday { get; set; }
        public string Banner { get; set; }
        public string Avatar { get; set; }       
        public string StudentId { get; set; }
        public string Introduction { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
    }
}
