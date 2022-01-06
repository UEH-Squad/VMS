using Microsoft.AspNetCore.Identity;

namespace VMS.Domain.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
