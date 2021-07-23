using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMS.Domain.Models;

namespace VMS.Infrastructure.Data.Context
{
    public class VmsDbContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<ProjectSkill> ProjectSkills { get; set; }
        public DbSet<ProjectVolunteer> ProjectVolunteers { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }

        public VmsDbContext(DbContextOptions<VmsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed roles to AspNetRoles table
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = "Organization", NormalizedName = "ORGANIZATION".ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "Volunteer", NormalizedName = "VOLUNTEER".ToUpper() });

            PasswordHasher<IdentityUser> hasher = new();

            //Seeding sample users to AspNetUsers table
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "admin@vms.com",
                    NormalizedUserName = "ADMIN@VMS.COM",
                    Email = "admin@vms.com",
                    NormalizedEmail = "ADMIN@VMS.COM",
                    PasswordHash = hasher.HashPassword(null, "Demo@123")
                }
            );
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                    UserName = "organization@vms.com",
                    NormalizedUserName = "ORGANIZATION@VMS.COM",
                    Email = "organization@vms.com",
                    NormalizedEmail = "ORGANIZATION@VMS.COM",
                    PasswordHash = hasher.HashPassword(null, "Demo@123")
                }
            );
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                    UserName = "volunteer@vms.com",
                    NormalizedUserName = "VOLUNTEER@VMS.COM",
                    Email = "volunteer@vms.com",
                    NormalizedEmail = "VOLUNTEER@VMS.COM",
                    PasswordHash = hasher.HashPassword(null, "Demo@123")
                }
            );

            //Seed the relation between our user and role to AspNetUserRoles table
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                }
            );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                }
            );

            // Seed categories
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Test category 1",
                    IsDeleted = false
                },
                new Category
                {
                    Id = 2,
                    Name = "Test category 2",
                    IsDeleted = false
                }
            );
        }
    }
}