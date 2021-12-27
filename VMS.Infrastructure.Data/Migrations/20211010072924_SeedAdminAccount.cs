using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class SeedAdminAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "bfedbf16-6192-4db9-9be9-edc7c5fae896");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "2a06db1b-9a7b-467d-89f7-23c9dd6c7687");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "df2b8eb2-edc7-4de7-b6d5-70eb28bb58df");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Avatar", "Banner", "Birthday", "Class", "ConcurrencyStamp", "Course", "CreatedBy", "CreatedDate", "Discriminator", "Email", "EmailConfirmed", "FacultyId", "FullAddress", "FullName", "Gender", "Introduction", "IsDeleted", "LockoutEnabled", "LockoutEnd", "Mission", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumber2", "PhoneNumberConfirmed", "Rank", "SecurityStamp", "StudentId", "TwoFactorEnabled", "UpdatedBy", "UpdatedDate", "UserName", "Website" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "128dcf2c-1853-451f-bcae-8d496abb79dc", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "hsv.ueh@ueh.edu.vn", true, null, null, null, null, null, false, false, null, null, "hsv.ueh@ueh.edu.vn", "admin", "AQAAAAEAACcQAAAAEE4iU8jRuvJutgfH5ott5OO2Vgjf9ylDO8Z1Pez4qsCIXx3QKLOYwCx42u16DiWlwg==", null, null, false, 0, "", null, false, null, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e570", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e570", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "5586e289-aadd-4126-9885-4eb6c59f1897");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "66ef1a6e-bd66-4e51-ac9d-8e5e6e82b419");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "0cc6be32-9169-4606-b692-760f7777d64c");
        }
    }
}
