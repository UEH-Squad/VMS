using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMS.Common;
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
        public DbSet<ActivityTarget> ActivityTargets { get; set; }
        public DbSet<ReasonReport> ReasonReports { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ImageReport> ImageReports { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Club> Clubs { get; set; }

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

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "a18be9c0-aa65-4af8-bd17-00bd9344e570",
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

            builder.Entity<Department>().HasData(
                    new Department { Id = 1, Name = "Ban Tổ chức - Xây dựng Đoàn" },
                    new Department { Id = 2, Name = "Ban Phong trào - Tình nguyện" },
                    new Department { Id = 3, Name = "Ban Học tập - Nghiên cứu khoa học - Quan hệ quốc tế" },
                    new Department { Id = 4, Name = "Ban Tổ chức - Xây dựng Hội" },
                    new Department { Id = 5, Name = "Ban Tình nguyện - Hỗ trợ sinh viên" }
                );

            builder.Entity<Club>().HasData(
                    new Club { Id = 1, Name = "Câu lạc bộ Anh Văn - BELL" },
                    new Club { Id = 2, Name = "Câu lạc bộ Bóng chuyền" },
                    new Club { Id = 3, Name = "Câu lạc bộ Dân ca" },
                    new Club { Id = 4, Name = "Câu lạc bộ Giai điệu trẻ" },
                    new Club { Id = 5, Name = "Câu lạc bộ Guitar - UEHG" },
                    new Club { Id = 6, Name = "Câu lạc bộ Tiếng Pháp - CFE" },
                    new Club { Id = 7, Name = "Câu lạc bộ Võ thuật" },
                    new Club { Id = 8, Name = "Câu lạc bộ Chuyện to nhỏ" },
                    new Club { Id = 9, Name = "Câu lạc bộ Dynamic" },
                    new Club { Id = 10, Name = "Đội Công tác xã hội" },
                    new Club { Id = 11, Name = "Đội Văn nghệ xung kích" },
                    new Club { Id = 12, Name = "Đội Cộng tác viên" },
                    new Club { Id = 13, Name = "Nhóm Truyền thông Sinh viên - S Communications" },
                    new Club { Id = 14, Name = "Câu lạc bộ Bất động sản - REC" },
                    new Club { Id = 15, Name = "Câu lạc bộ Chuyên viên Nhân sự Tập sự - HuReA" },
                    new Club { Id = 16, Name = "Câu lạc bộ Chứng khoán - SCUE" },
                    new Club { Id = 17, Name = "Câu lạc bộ Công nghệ Kinh tế - ET Group" },
                    new Club { Id = 18, Name = "Câu lạc bộ Kế toán - Kiểm toán A²C" },
                    new Club { Id = 19, Name = "Câu lạc bộ Kinh doanh quốc tế - IBC" },
                    new Club { Id = 20, Name = "Câu lạc bộ Nghiên cứu Kinh tế Trẻ - YoRE" },
                    new Club { Id = 21, Name = "Câu lạc bộ Nhân Sự Khởi Nghiệp - HR Startup" },
                    new Club { Id = 22, Name = "Câu lạc bộ Pháp lý" },
                    new Club { Id = 23, Name = "Câu lạc bộ Thương mại - IC" },
                    new Club { Id = 24, Name = "Câu lạc bộ Tiếng Anh - Apple Club" },
                    new Club { Id = 25, Name = "Câu lạc bộ Lý luận trẻ" },
                    new Club { Id = 26, Name = "Nhóm Hỗ Trợ Sinh Viên - SSG" },
                    new Club { Id = 27, Name = "Nhóm Sinh viên Nghiên cứu Marketing - Margroup" },
                    new Club { Id = 28, Name = "Nhóm Sinh Viên Nghiên Cứu Tài Chính - SFR" },
                    new Club { Id = 29, Name = "Nhóm Sinh Viên Nghiên Cứu Thuế - TaxGroup" },
                    new Club { Id = 30, Name = "Nhóm Sinh viên Nghiên cứu Du lịch - Travel Group" }
                );
        }
    }
}