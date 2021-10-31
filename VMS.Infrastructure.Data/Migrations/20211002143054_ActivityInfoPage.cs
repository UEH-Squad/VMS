using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class ActivityInfoPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "445034e8-bea2-4693-803e-811277f3a2de");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "d2ac63c5-f33e-4602-8a3b-b050359cbb08");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "5b73e236-a1fa-484e-95fc-0311f1c3a683");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "1bd56f89-50e9-4e96-9d16-23d27e5f9a60");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "f284a806-7ad2-4db8-b5fb-f347cd4bb0db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "1b4b423d-ce72-433d-927a-17e52c32f09f");
        }
    }
}
