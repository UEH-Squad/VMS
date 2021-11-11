using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class _seedingOrgData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "a5328e7a-0d2c-421b-92e0-31520a8ad9f7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "14304e5d-bcae-477e-ba3c-6faf2e37a34c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "99fdfc18-f94a-47e2-b446-398fc9ec545b");

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 30, "Nhóm Sinh viên Nghiên cứu Du lịch - Travel Group" },
                    { 29, "Nhóm Sinh Viên Nghiên Cứu Thuế - TaxGroup" },
                    { 28, "Nhóm Sinh Viên Nghiên Cứu Tài Chính - SFR" },
                    { 27, "Nhóm Sinh viên Nghiên cứu Marketing - Margroup" },
                    { 26, "Nhóm Hỗ Trợ Sinh Viên - SSG" },
                    { 25, "Câu lạc bộ Lý luận trẻ" },
                    { 24, "Câu lạc bộ Tiếng Anh - Apple Club" },
                    { 23, "Câu lạc bộ Thương mại - IC" },
                    { 22, "Câu lạc bộ Pháp lý" },
                    { 21, "Câu lạc bộ Nhân Sự Khởi Nghiệp - HR Startup" },
                    { 20, "Câu lạc bộ Nghiên cứu Kinh tế Trẻ - YoRE" },
                    { 18, "Câu lạc bộ Kế toán - Kiểm toán A²C" },
                    { 17, "Câu lạc bộ Công nghệ Kinh tế - ET Group" },
                    { 19, "Câu lạc bộ Kinh doanh quốc tế - IBC" },
                    { 15, "Câu lạc bộ Chuyên viên Nhân sự Tập sự - HuReA" },
                    { 2, "Câu lạc bộ Bóng chuyền" },
                    { 3, "Câu lạc bộ Dân ca" },
                    { 4, "Câu lạc bộ Giai điệu trẻ" },
                    { 16, "Câu lạc bộ Chứng khoán - SCUE" },
                    { 6, "Câu lạc bộ Tiếng Pháp - CFE" },
                    { 7, "Câu lạc bộ Võ thuật" },
                    { 5, "Câu lạc bộ Guitar - UEHG" },
                    { 9, "Câu lạc bộ Dynamic" },
                    { 10, "Đội Công tác xã hội" },
                    { 11, "Đội Văn nghệ xung kích" },
                    { 12, "Đội Cộng tác viên" },
                    { 13, "Nhóm Truyền thông Sinh viên - S Communications" },
                    { 14, "Câu lạc bộ Bất động sản - REC" },
                    { 8, "Câu lạc bộ Chuyện to nhỏ" },
                    { 1, "Câu lạc bộ Anh Văn - BELL" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ban Tổ chức - Xây dựng Đoàn" },
                    { 2, "Ban Phong trào - Tình nguyện" },
                    { 3, "Ban Học tập - Nghiên cứu khoa học - Quan hệ quốc tế" },
                    { 4, "Ban Tổ chức - Xây dựng Hội" },
                    { 5, "Ban Tình nguyện - Hỗ trợ sinh viên" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "5ffcef2a-5b15-4186-9c27-119ba3edf5ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "ff1b571c-17fe-4513-a65f-6039498cf32d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "09c6c292-df64-4d53-b422-6a2e8726e25a");
        }
    }
}
