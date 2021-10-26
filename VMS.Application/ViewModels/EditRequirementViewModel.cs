using System.ComponentModel.DataAnnotations;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    public class EditRequirementViewModel
    {

        [Required]
        public string Content { get; set; }
    }
}
