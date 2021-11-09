namespace VMS.Application.ViewModels
{
    public class CreateAccountViewModel
    {
        public string StudentId { get; set; }
        public string PasswordHash { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public string Email { get; set; }
    }
}
