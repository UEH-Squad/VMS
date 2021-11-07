using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    /// <summary>
    /// Base class
    /// </summary>
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        [Required] public string FullName { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string NotifiedEmail { get; set; }
        [Required] public string PhoneNumber { get; set; }

        public string Banner { get; set; }
        public string Avatar { get; set; }

        [RequiredHasItems]
        public IList<AreaViewModel> Areas { get; set; } = new List<AreaViewModel>();
    }

    public class CreateOrgProfileViewModel : UserProfileViewModel
    {
        [Required] public string Mission { get; set; }
    }

    public class CreateUserProfileViewModel : UserProfileViewModel
    {
        [RequiredGreaterThanZero] public string? FacultyId { get; set; }
        public string FacultyName { get; set; }
        [Required] public string Class { get; set; }
        public string Course { get; set; }

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
        public string StudentId { get; set; }
        public string Introduction { get; set; }

        [RequiredHasItems]
        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
    }
}