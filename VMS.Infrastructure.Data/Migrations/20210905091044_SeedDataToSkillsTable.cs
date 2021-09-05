using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class SeedDataToSkillsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Kiến thức chuyên ngành");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Kỹ năng mềm");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Hoạch định tài chính");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Quản lý thời gian");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Chỉnh sửa/Thiết kế hình ảnh/Video");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Thể lực tốt");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Viết Proposal/Kịch bản/Content");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Lập trình");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Xây dựng website");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Lái xe");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Luật", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Kế toán/Kiểm toán", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Marketing", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Quản trị", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Tài chính", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "Ngân hàng");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "Ngoại ngữ");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Làm việc nhóm", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Tư duy Logic", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Xây dựng kế hoạch", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Giao tiếp & Ứng xử", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Giải quyết vấn đề", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Đồng cảm & Sẻ chia", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Quan sát & Lắng nghe", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Tìm kiếm & Xử lý thông tin", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Kiểm soát cảm xúc");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Kiên nhẫn");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "Chăm chỉ");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "Siêng năng");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Kỹ năng mềm");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Kiến thức chuyên ngành");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Siêng năng");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Lập trình");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Có phương tiện di chuyển");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Kiên nhẫn");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Thể lực tốt");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Hoạch định tài chính");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Nhiệt tình");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Trách nhiệm");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Chỉnh sửa/Thiết kế hình ảnh/video", null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Checklist", null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Xây dựng website", null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Viết Proposal/kịch bản/content", null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Lái xe", null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "Làm việc nhóm");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "Tư duy Logic");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Xây dựng kế hoạch", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Giao tiếp & Ứng xử", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Giải quyết vấn đề", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Quản lý thời gian", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Đồng cảm & Sẻ chia", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Quan sát & Lắng nghe", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Tìm kiếm & Xử lý thông tin", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "ParentSkillId" },
                values: new object[] { "Kiểm soát cảm xúc", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Luật");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Kế toán/Kiểm toán");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "Marketing");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "Quản trị");

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentSkillId" },
                values: new object[,]
                {
                    { 32, false, "Ngoại ngữ", 2 },
                    { 31, false, "Ngân hàng", 2 },
                    { 33, false, "Lý luận chính trị", 2 },
                    { 30, false, "Tài chính", 2 }
                });
        }
    }
}
