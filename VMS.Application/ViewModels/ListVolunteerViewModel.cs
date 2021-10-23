using System;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class ListVolunteerViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Desire { get; set; }
        public bool IsCommit { get; set; }
        public bool IsCheck { get; set; }

        public User User { get; set; }
    }
}
