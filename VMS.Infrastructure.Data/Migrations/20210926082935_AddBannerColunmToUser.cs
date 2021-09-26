using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class AddBannerColunmToUser : Migration
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
                value: "aa4856e2-1c52-41bb-924b-d1f183086516");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "516b6390-0b6d-47ea-9faf-65b653137d5d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "b388361a-26c3-4588-ba25-239dab4ef23a");
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
                value: "94fa6164-162f-4746-971e-777e9ac704f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "e01c8442-8909-44ee-bf11-492141554a34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "96ea313b-4aaa-4034-a949-ae740638150a");
        }
    }
}
