using System.ComponentModel.DataAnnotations;
using VMS.Common.CustomValidations;

namespace VMS.Application.ViewModels
{
    public class SignUpActivityViewModel
    {
        public string UserId { get; set; }

        public string ActivityId { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [IsValidPhoneNumber(ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Desire { get; set; }

        [Required]
        public bool IsCommit { get; set; }
    }
}
