using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class CreateAccountViewModel
    {
        public string FullName { get; set; }

        public string Class { get; set; }
        public string StudentId { get; set; }
        public string Course { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        [EmailAddress] public string Email { get; set; }
    }
}
