using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class SeedDataToAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: "people_outline");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "pan_tool", "Hỗ trợ" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "import_contacts", "Giáo dục" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "format_shapes", "Kỹ thuật" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "local_hospital", "Sức khỏe" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "drive_eta", "Phương tiện" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "wb_sunny", "Môi trường" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "directions_bike", "Thể thao" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "notifications_active", "Khẩn cấp" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "calendar_today", "Sự kiện" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Icon",
                value: "home");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Icon", "Name" },
                values: new object[] { "computer", "Công nghệ" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: null);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Sự kiện" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Hỗ trợ" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Giáo dục" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Khẩn cấp" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Kỹ thuật" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Sức khỏe" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Công nghệ" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Phương tiện" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Môi trường" });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Icon",
                value: null);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Icon", "Name" },
                values: new object[] { null, "Thể thao" });
        }
    }
}
