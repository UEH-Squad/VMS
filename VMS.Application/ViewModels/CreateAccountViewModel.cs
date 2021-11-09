using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class CreateAccountViewModel
    {
        public string Class { get; set; }

        [Required] public string StudentId { get; set; }

        [Required] public string FullName { get; set; }

        [Required] public string Course { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
