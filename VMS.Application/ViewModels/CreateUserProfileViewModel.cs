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
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
           
        [RequiredHasItems]
        public IList<AreaViewModel> Areas { get; set; } = new List<AreaViewModel>();

        [Required]
        public string Mission { get; set; }

        public string Banner { get; set; }

        public string Avatar { get; set; }
    }
}
