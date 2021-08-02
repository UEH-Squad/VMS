using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMS.Domain.Models;

namespace VMS.Infrastructure.Data.Context
{
    public class VmsDbContext : IdentityDbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAddress> ActivityAddresses { get; set; }
        public DbSet<ActivityImage> ActivityImages { get; set; }
        public DbSet<ActivityRequirement> ActivityRequirements { get; set; }
        public DbSet<ActivitySkill> ActivitySkills { get; set; }
        public DbSet<AddressPath> AddressPaths { get; set; }
        public DbSet<AddressPathType> AddressPathTypes { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<RecruitmentRating> RecruitmentRatings { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserArea> UserAreas { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }

        public VmsDbContext(DbContextOptions<VmsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(x => x.Activities)
                .WithOne(x => x.Organizer)
                .HasForeignKey(x => x.OrgId);

            builder.Entity<User>()
                .HasMany(x => x.ActivityApprovals)
                .WithOne(x => x.Approver)
                .HasForeignKey(x => x.ApprovedBy);

            builder.Entity<AddressPath>()
                .HasOne(x => x.PreviousPath)
                .WithMany(x => x.SubPaths)
                .HasForeignKey(x => x.ParentPathId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            // add demo data for skills and requirements
            builder.Entity<Skill>().HasData(
                    new Skill
                    {
                        Id = 1,
                        Name = "C#/.NET",
                        IsDeleted = false
                    },
                    new Skill
                    {
                        Id = 2,
                        Name = "HTML/CSS",
                        IsDeleted = false
                    },
                    new Skill
                    {
                        Id = 3,
                        Name = "Java",
                        IsDeleted = false
                    }
                );
            builder.Entity<Requirement>().HasData(
                    new Requirement
                    {
                        Id = 1,
                        Name = "Văn hay chữ tốt",
                        IsDeleted = false
                    },
                    new Skill
                    {
                        Id = 2,
                        Name = "Bậc thầy tính toán",
                        IsDeleted = false
                    },
                    new Skill
                    {
                        Id = 3,
                        Name = "Phù thủy hóa học",
                        IsDeleted = false
                    }
                );
            builder.Entity<Area>().HasData(
                new Area
                {
                    Id = 1,
                    Name = "Công nghệ thông tin",
                    IsDeleted = false
                },
                new Area
                {
                    Id = 2,
                    Name = "Marketing",
                    IsDeleted = false
                },
                new Area
                {
                    Id = 3,
                    Name = "Cộng đồng",
                    IsDeleted = false
                }
            );
        }
    }
}