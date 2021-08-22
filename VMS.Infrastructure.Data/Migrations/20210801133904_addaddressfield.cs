using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class addaddressfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 1, false, "Công nghệ thông tin" });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 2, false, "Marketing" });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 3, false, "Cộng đồng" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Activities");
        }
    }
}
