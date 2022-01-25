using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class UpdateSettingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoHomepageURL",
                table: "Settings",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "Key");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "6f2b8357-bb81-4592-8ec3-10155d8fd8e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "58971f87-5687-450b-a333-3805a8ce42fe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "6f20476c-0770-4f28-8ea0-ab996a0d8b80");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "941836e4-bdaf-4f18-ada1-d956a5869beb", "AQAAAAEAACcQAAAAEEhJf1pbO2lr7oVKPwPsJK9f1eBTj3e2927AthbN7wZl33YT4OcDt9aX0WvcTfYzJg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Settings",
                newName: "VideoHomepageURL");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "53911d31-8866-41ec-8cfb-3189de1f0c2a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "3140a680-1645-4ff7-aa93-541d5e6dd59b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "a4050287-e973-48ab-bd07-2645f2443bed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1ff155c5-a6ab-4a63-bcf9-51ecbbe91540", "AQAAAAEAACcQAAAAEPFSmjipCqoYRsdAtnxp56yc/TmDh5WScMtLWstUEhgzXe+gfuAWbRiGzCmuhvo45Q==" });
        }
    }
}
