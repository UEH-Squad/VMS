using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class AddNewSkillsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentSkillId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Cộng đồng");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Sự kiện");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Hỗ trợ");

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Icon", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 4, null, false, "Giáo dục" },
                    { 5, null, false, "Khẩn cấp" },
                    { 6, null, false, "Kỹ thuật" },
                    { 7, null, false, "Sức khỏe" },
                    { 8, null, false, "Công nghệ" },
                    { 9, null, false, "Phương tiện" },
                    { 10, null, false, "Môi trường" },
                    { 11, null, false, "Chuyển nhà" },
                    { 12, null, false, "Thể thao" }
                });

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

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentSkillId" },
                values: new object[,]
                {
                    { 10, false, "Trách nhiệm", null },
                    { 4, false, "Lập trình", null },
                    { 5, false, "Có phương tiện di chuyển", null },
                    { 6, false, "Kiên nhẫn", null },
                    { 7, false, "Thể lực tốt", null },
                    { 33, false, "Lý luận chính trị", 2 },
                    { 32, false, "Ngoại ngữ", 2 },
                    { 31, false, "Ngân hàng", 2 },
                    { 30, false, "Tài chính", 2 },
                    { 29, false, "Quản trị", 2 },
                    { 28, false, "Marketing", 2 },
                    { 27, false, "Kế toán/Kiểm toán", 2 },
                    { 26, false, "Luật", 2 },
                    { 25, false, "Kiểm soát cảm xúc", 1 },
                    { 24, false, "Tìm kiếm & Xử lý thông tin", 1 },
                    { 23, false, "Quan sát & Lắng nghe", 1 },
                    { 22, false, "Đồng cảm & Sẻ chia", 1 },
                    { 21, false, "Quản lý thời gian", 1 },
                    { 20, false, "Giải quyết vấn đề", 1 },
                    { 19, false, "Giao tiếp & Ứng xử", 1 },
                    { 18, false, "Xây dựng kế hoạch", 1 },
                    { 8, false, "Hoạch định tài chính", null },
                    { 16, false, "Làm việc nhóm", 1 },
                    { 15, false, "Lái xe", null },
                    { 14, false, "Viết Proposal/kịch bản/content", null },
                    { 13, false, "Xây dựng website", null },
                    { 12, false, "Checklist", null }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentSkillId" },
                values: new object[] { 11, false, "Chỉnh sửa/Thiết kế hình ảnh/video", null });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentSkillId" },
                values: new object[] { 9, false, "Nhiệt tình", null });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentSkillId" },
                values: new object[] { 17, false, "Tư duy Logic", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ParentSkillId",
                table: "Skills",
                column: "ParentSkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skills_ParentSkillId",
                table: "Skills",
                column: "ParentSkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skills_ParentSkillId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ParentSkillId",
                table: "Skills");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 29);

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

            migrationBuilder.DropColumn(
                name: "ParentSkillId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Areas");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Công nghệ thông tin");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Marketing");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Cộng đồng");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "C#/.NET");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "HTML/CSS");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Java");
        }
    }
}
