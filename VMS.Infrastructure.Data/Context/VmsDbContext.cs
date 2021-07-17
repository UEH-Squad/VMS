using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VMS.Infrastructure.Data.Context
{
    public class VmsDbContext : IdentityDbContext
    {
        public VmsDbContext(DbContextOptions<VmsDbContext> options) : base(options)
        {
        }
    }
}