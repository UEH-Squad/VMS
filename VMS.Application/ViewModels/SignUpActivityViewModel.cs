﻿using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class SignUpActivityViewModel
    {
        public string UserId { get; set; }

        public string ActivityId { get; set; }

        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Desire { get; set; }

        [Required]
        public bool IsCommit { get; set; }
    }
}