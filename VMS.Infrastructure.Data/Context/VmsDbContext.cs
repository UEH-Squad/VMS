using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMS.Common.Enums;
using VMS.Domain.Models;

namespace VMS.Infrastructure.Data.Context
{
    public class VmsDbContext : IdentityDbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAddress> ActivityAddresses { get; set; }
        public DbSet<ActivityImage> ActivityImages { get; set; }
        public DbSet<ActivitySkill> ActivitySkills { get; set; }
        public DbSet<AddressPath> AddressPaths { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<RecruitmentRating> RecruitmentRatings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserArea> UserAreas { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ActivityTarget> ActivityTargets { get; set; }
        public DbSet<ReasonReport> ReasonReports { get; set; }
        public DbSet<ImageReport> ImageReports { get; set; }
        public DbSet<Faculty> Faculties { get; set; }

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

            builder.Entity<Skill>()
                .HasOne(x => x.ParentSkill)
                .WithMany(x => x.SubSkills)
                .HasForeignKey(x => x.ParentSkillId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<Favorite>()
                .Property(f => f.UserId).IsRequired();
            builder.Entity<Favorite>()
                .Property(f => f.ActivityId).IsRequired();

            builder.Entity<ImageReport>()
                .HasOne(e => e.RecruitmentRating)
                .WithMany(e => e.ImageReports)
                .HasForeignKey(e => e.RecruitmentRatingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ImageReport>()
                .HasOne(e => e.Feedback)
                .WithMany(e => e.ImageReports)
                .HasForeignKey(e => e.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReasonReport>()
                .HasOne(e => e.RecruitmentRating)
                .WithMany(e => e.ReasonReports)
                .HasForeignKey(e => e.RecruitmentRatingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReasonReport>()
                .HasOne(e => e.Feedback)
                .WithMany(e => e.ReasonReports)
                .HasForeignKey(e => e.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Skill>().HasData(
                    new Skill { Id = 1, Name = "Kiến thức chuyên ngành" },
                    new Skill { Id = 2, Name = "Kỹ năng mềm" },
                    new Skill { Id = 3, Name = "Hoạch định tài chính" },
                    new Skill { Id = 4, Name = "Quản lý thời gian" },
                    new Skill { Id = 5, Name = "Chỉnh sửa/Thiết kế hình ảnh/Video" },
                    new Skill { Id = 6, Name = "Thể lực tốt" },
                    new Skill { Id = 7, Name = "Viết Proposal/Kịch bản/Content" },
                    new Skill { Id = 8, Name = "Lập trình" },
                    new Skill { Id = 9, Name = "Xây dựng website" },
                    new Skill { Id = 10, Name = "Lái xe" },

                    new Skill { Id = 11, Name = "Luật", ParentSkillId = 1 },
                    new Skill { Id = 12, Name = "Kế toán/Kiểm toán", ParentSkillId = 1 },
                    new Skill { Id = 13, Name = "Marketing", ParentSkillId = 1 },
                    new Skill { Id = 14, Name = "Quản trị", ParentSkillId = 1 },
                    new Skill { Id = 15, Name = "Tài chính", ParentSkillId = 1 },
                    new Skill { Id = 16, Name = "Ngân hàng", ParentSkillId = 1 },
                    new Skill { Id = 17, Name = "Ngoại ngữ", ParentSkillId = 1 },

                    new Skill { Id = 18, Name = "Làm việc nhóm", ParentSkillId = 2 },
                    new Skill { Id = 19, Name = "Tư duy Logic", ParentSkillId = 2 },
                    new Skill { Id = 20, Name = "Xây dựng kế hoạch", ParentSkillId = 2 },
                    new Skill { Id = 21, Name = "Giao tiếp & Ứng xử", ParentSkillId = 2 },
                    new Skill { Id = 22, Name = "Giải quyết vấn đề", ParentSkillId = 2 },
                    new Skill { Id = 23, Name = "Đồng cảm & Sẻ chia", ParentSkillId = 2 },
                    new Skill { Id = 24, Name = "Quan sát & Lắng nghe", ParentSkillId = 2 },
                    new Skill { Id = 25, Name = "Tìm kiếm & Xử lý thông tin", ParentSkillId = 2 },
                    new Skill { Id = 26, Name = "Kiểm soát cảm xúc", ParentSkillId = 2 },
                    new Skill { Id = 27, Name = "Kiên nhẫn", ParentSkillId = 2 },
                    new Skill { Id = 28, Name = "Chăm chỉ", ParentSkillId = 2 },
                    new Skill { Id = 29, Name = "Siêng năng", ParentSkillId = 2 }
                );

            builder.Entity<Area>().HasData(
                    new Area { Id = 1, Name = "Cộng đồng", Icon = "people_outline" },
                    new Area { Id = 2, Name = "Hỗ trợ", Icon = "pan_tool" },
                    new Area { Id = 3, Name = "Giáo dục", Icon = "import_contacts" },
                    new Area { Id = 4, Name = "Kỹ thuật", Icon = "format_shapes" },
                    new Area { Id = 5, Name = "Sức khỏe", Icon = "local_hospital" },
                    new Area { Id = 6, Name = "Phương tiện", Icon = "drive_eta" },
                    new Area { Id = 7, Name = "Môi trường", Icon = "wb_sunny" },
                    new Area { Id = 8, Name = "Thể thao", Icon = "directions_bike" },
                    new Area { Id = 9, Name = "Khẩn cấp", Icon = "notifications_active" },
                    new Area { Id = 10, Name = "Sự kiện", Icon = "calendar_today" },
                    new Area { Id = 11, Name = "Chuyển nhà", Icon = "home" },
                    new Area { Id = 12, Name = "Công nghệ", Icon = "computer" }
            );

            const string ADMIN_ROLE_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e570";
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = Role.Admin.ToString(),
                NormalizedName = Role.Admin.ToString()
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                Name = Role.Organization.ToString(),
                NormalizedName = Role.Organization.ToString()
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                Name = Role.User.ToString(),
                NormalizedName = Role.User.ToString()
            });

            // seed admin account
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";

            PasswordHasher<User> hasher = new();
            builder.Entity<User>().HasData(new User
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "hsv.ueh@ueh.edu.vn",
                NormalizedEmail = "hsv.ueh@ueh.edu.vn",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            });

            builder.Entity<Faculty>()
                .HasMany(e => e.Users)
                .WithOne(e => e.Faculty)
                .HasForeignKey(e => e.FacultyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Faculty>().HasData(
                    new Faculty { Id = 1, Name = "Khoa Luật" },
                    new Faculty { Id = 2, Name = "Khoa Kế toán" },
                    new Faculty { Id = 3, Name = "Khoa Kinh tế" },
                    new Faculty { Id = 4, Name = "Khoa Khoa học xã hội" },
                    new Faculty { Id = 5, Name = "Khoa Ngân hàng" },
                    new Faculty { Id = 6, Name = "Khoa Ngoại ngữ" },
                    new Faculty { Id = 7, Name = "Khoa Quản lý nhà nước" },
                    new Faculty { Id = 8, Name = "Khoa Quản trị" },
                    new Faculty { Id = 9, Name = "Khoa Tài chính" },
                    new Faculty { Id = 10, Name = "Khoa Tài chính công" },
                    new Faculty { Id = 11, Name = "Khoa Công nghệ thông tin kinh doanh" },
                    new Faculty { Id = 12, Name = "Khoa Kinh doanh quốc tế - Marketing" },
                    new Faculty { Id = 13, Name = "Khoa Toán - Thống kê" },
                    new Faculty { Id = 14, Name = "Viện Du lịch" },
                    new Faculty { Id = 15, Name = "Viện Đào tạo quốc tế" }
                );
        }
    }
}