using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class _addBannerforOrg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Banner",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "9805fbb4-1308-48bd-8ffa-44f09aa83536");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "8a7d66e7-4848-4d51-b159-b0caa59a1113");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "0ef877ce-8196-455d-9373-8cbffb074d99");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banner",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "37672a6a-cb52-4c00-9340-493aa9380e64");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "eab96a04-cb40-4b0a-958f-9db14c25e42b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "a34e60f8-26db-4e0a-b18c-70f7db95b127");
        }
    }
}
