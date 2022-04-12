using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class RenameIsBothChoiceFieldActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBothChoice",
                table: "Activities",
                newName: "IsSingleChoice");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "db4d1b63-2b91-4d94-9e47-6327e0d3aac1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "b339282d-bdb4-4a46-ae23-02663d718963");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "f32a1b5e-723e-462c-b33d-4986d2c51ada");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a9388752-17d1-456d-877a-70424d9d4e4e", "AQAAAAEAACcQAAAAEFG2ojID0srEJdCSEpjuG29jwkLpW2v8XUPdtCvdezFTgVn2jaka1S5XUXVSyJ1ePw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSingleChoice",
                table: "Activities",
                newName: "IsBothChoice");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "04d3cf0a-f1ef-46db-b797-7661d141c593");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "c01a181a-ceee-45b4-83cd-c3f32555b040");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "2a0aacaf-2b05-469b-a012-069cb1dee37a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "05b2de43-9224-48fb-b39c-2eb89b356f1e", "AQAAAAEAACcQAAAAEN7E6m6p+sUSRuqBRofnhmVzpl+cfEwjK7J6eyJD50hFg5+aJQX9geQCayVH5/D31g==" });
        }
    }
}
