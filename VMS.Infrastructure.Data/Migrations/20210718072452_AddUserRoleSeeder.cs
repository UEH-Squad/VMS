using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class AddUserRoleSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", "4ca5ff4b-5cfa-45f1-b8a5-7c839091d29d", "Administrator", "ADMINISTRATOR" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7211", "d9cf6aba-f4df-4771-a2fd-25c047250d4e", "Organization", "ORGANIZATION" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7212", "a7f43ae9-0a81-4105-b6a1-1b09ab9daee9", "Volunteer", "VOLUNTEER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "723b34f3-6172-4942-bafd-8caa08393923", "IdentityUser", "admin@vms.com", false, false, null, "ADMIN@VMS.COM", "ADMIN@VMS.COM", "AQAAAAEAACcQAAAAEIx+crfvcGShINOpB5laEXO+aos5Jx91Z8rWTZbY+A6eT/rT+HG7WdaxYbkmRRJI4Q==", null, false, "dc0c0ef8-df4a-4a84-9e3a-be7322041907", false, "admin@vms.com" },
                    { "8e445865-a24d-4543-a6c6-9443d048cdb8", 0, "e5584e31-8e77-43ba-b544-d8bb647e2da7", "IdentityUser", "organization@vms.com", false, false, null, "ORGANIZATION@VMS.COM", "ORGANIZATION@VMS.COM", "AQAAAAEAACcQAAAAEONp1aG8NQt2z7MhVXbntiCs06NkXvCITo3wzLei2sOvx3mT1+1+WWjdqR3gB8I5+w==", null, false, "a11a70f9-1d93-4f1f-9b08-7f5947a8e275", false, "organization@vms.com" },
                    { "8e445865-a24d-4543-a6c6-9443d048cdb7", 0, "75f50c50-e3bd-4f58-9883-ee4d7cac7f60", "IdentityUser", "volunteer@vms.com", false, false, null, "VOLUNTEER@VMS.COM", "VOLUNTEER@VMS.COM", "AQAAAAEAACcQAAAAEIv1I7HMM5tV5yNc7X49/ZibSWqqWgs5kjjjcL4jz/LaScIPUWvp4BcRdWAeABcawg==", null, false, "47507e9c-abd7-41ed-b8cb-f06a872ff0e9", false, "volunteer@vms.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", "2c5e174e-3b0e-446f-86af-483d56fd7210" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb8", "2c5e174e-3b0e-446f-86af-483d56fd7211" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb7", "2c5e174e-3b0e-446f-86af-483d56fd7212" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb7", "2c5e174e-3b0e-446f-86af-483d56fd7212" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb8", "2c5e174e-3b0e-446f-86af-483d56fd7211" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", "2c5e174e-3b0e-446f-86af-483d56fd7210" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7211");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7212");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
