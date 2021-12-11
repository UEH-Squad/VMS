using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class AppRole : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
