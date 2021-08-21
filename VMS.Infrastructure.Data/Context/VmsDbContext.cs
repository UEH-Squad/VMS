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

            // add demo data for skills and requirements
            builder.Entity<Skill>().HasData(
                    new Skill { Id = 1, Name = "Kỹ năng mềm" },
                    new Skill { Id = 2, Name = "Kiến thức chuyên ngành" },
                    new Skill { Id = 3, Name = "Siêng năng" },
                    new Skill { Id = 4, Name = "Lập trình" },
                    new Skill { Id = 5, Name = "Có phương tiện di chuyển" },
                    new Skill { Id = 6, Name = "Kiên nhẫn" },
                    new Skill { Id = 7, Name = "Thể lực tốt" },
                    new Skill { Id = 8, Name = "Hoạch định tài chính" },
                    new Skill { Id = 9, Name = "Nhiệt tình" },
                    new Skill { Id = 10, Name = "Trách nhiệm" },
                    new Skill { Id = 11, Name = "Chỉnh sửa/Thiết kế hình ảnh/video" },
                    new Skill { Id = 12, Name = "Checklist" },
                    new Skill { Id = 13, Name = "Xây dựng website" },
                    new Skill { Id = 14, Name = "Viết Proposal/kịch bản/content" },
                    new Skill { Id = 15, Name = "Lái xe" },
                    new Skill { Id = 16, Name = "Làm việc nhóm", ParentSkillId = 1 },
                    new Skill { Id = 17, Name = "Tư duy Logic", ParentSkillId = 1 },
                    new Skill { Id = 18, Name = "Xây dựng kế hoạch", ParentSkillId = 1 },
                    new Skill { Id = 19, Name = "Giao tiếp & Ứng xử", ParentSkillId = 1 },
                    new Skill { Id = 20, Name = "Giải quyết vấn đề", ParentSkillId = 1 },
                    new Skill { Id = 21, Name = "Quản lý thời gian", ParentSkillId = 1 },
                    new Skill { Id = 22, Name = "Đồng cảm & Sẻ chia", ParentSkillId = 1 },
                    new Skill { Id = 23, Name = "Quan sát & Lắng nghe", ParentSkillId = 1 },
                    new Skill { Id = 24, Name = "Tìm kiếm & Xử lý thông tin", ParentSkillId = 1 },
                    new Skill { Id = 25, Name = "Kiểm soát cảm xúc", ParentSkillId = 1 },
                    new Skill { Id = 26, Name = "Luật", ParentSkillId = 2 },
                    new Skill { Id = 27, Name = "Kế toán/Kiểm toán", ParentSkillId = 2 },
                    new Skill { Id = 28, Name = "Marketing", ParentSkillId = 2 },
                    new Skill { Id = 29, Name = "Quản trị", ParentSkillId = 2 },
                    new Skill { Id = 30, Name = "Tài chính", ParentSkillId = 2 },
                    new Skill { Id = 31, Name = "Ngân hàng", ParentSkillId = 2 },
                    new Skill { Id = 32, Name = "Ngoại ngữ", ParentSkillId = 2 },
                    new Skill { Id = 33, Name = "Lý luận chính trị", ParentSkillId = 2 }
                );
            builder.Entity<Area>().HasData(
                    new Area { Id = 1, Name = "Cộng đồng" },
                    new Area { Id = 2, Name = "Sự kiện" },
                    new Area { Id = 3, Name = "Hỗ trợ" },
                    new Area { Id = 4, Name = "Giáo dục" },
                    new Area { Id = 5, Name = "Khẩn cấp" },
                    new Area { Id = 6, Name = "Kỹ thuật" },
                    new Area { Id = 7, Name = "Sức khỏe" },
                    new Area { Id = 8, Name = "Công nghệ" },
                    new Area { Id = 9, Name = "Phương tiện" },
                    new Area { Id = 10, Name = "Môi trường" },
                    new Area { Id = 11, Name = "Chuyển nhà" },
                    new Area { Id = 12, Name = "Thể thao" }
            );
        }
    }
}