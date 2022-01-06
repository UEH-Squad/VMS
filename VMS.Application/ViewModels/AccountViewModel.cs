using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public string Faculty { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string StudentId { get; set; }
        public DateTime CreatedDate { get; set; }

        [EmailAddress] public string Email { get; set; }

        public void Copy(AccountViewModel account)
        {
            Id = account.Id;
            Class = account.Class;
            Email = account.Email;
            Course = account.Course;
            FullName = account.FullName;
            UserName = account.UserName;
            Password = account.Password;
            StudentId = account.StudentId;
            CreatedDate = account.CreatedDate;
        }
    }
}
