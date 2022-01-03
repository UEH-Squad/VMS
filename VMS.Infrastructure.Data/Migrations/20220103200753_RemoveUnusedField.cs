using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class RemoveUnusedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartToFix",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDenied",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "NumberOfDay",
                table: "Activities",
                newName: "NumberOfDays");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "5f2af702-61d4-4a38-93df-90d418cf03eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "d1cd795f-5519-4b4b-b06b-18628a46900a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "0b890143-d2c8-48c0-8713-52956ddfc180");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "03bfb1ac-889a-4979-abfc-1e20ed3e3702", "AQAAAAEAACcQAAAAEPkZC+fgIl+lzRhcEk8kSAhUyGcC7OosTqufPiEhlB29R5MZK9IYEmRQltHqnlEDkg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfDays",
                table: "Activities",
                newName: "NumberOfDay");

            migrationBuilder.AddColumn<string>(
                name: "PartToFix",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDenied",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "7ba030d6-1286-4f5a-9bf8-4c30fe673b41");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "7928bc4d-918c-4f06-bfca-3f4c830f5c7e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "76ff8748-7a38-46f8-80c2-717a44932672");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a14d9b06-54aa-4dda-ba14-17f9518321c6", "AQAAAAEAACcQAAAAEPDiN+4UdbpNMdFun2kmTr/EzMgbvwWUJGkbBynWPwbIbK/+F3KQFz7KI01AK1z/XQ==" });
        }
    }
}
