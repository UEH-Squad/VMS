using Microsoft.AspNetCore.Identity;

namespace VMS.Domain.Models
{
    public class Volunteer : IdentityUser
    {
        public bool IsDeleted { get; set; }
    }
}