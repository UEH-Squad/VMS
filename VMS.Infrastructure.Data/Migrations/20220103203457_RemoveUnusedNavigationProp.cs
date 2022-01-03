using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class RemoveUnusedNavigationProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ImageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_ReasonReports_RecruitmentRatingId",
                table: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_ImageReports_RecruitmentRatingId",
                table: "ImageReports");

            migrationBuilder.DropColumn(
                name: "IsReport",
                table: "RecruitmentRatings");

            migrationBuilder.DropColumn(
                name: "RecruitmentRatingId",
                table: "ReasonReports");

            migrationBuilder.DropColumn(
                name: "RecruitmentRatingId",
                table: "ImageReports");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "d8a35366-4fcc-494e-ab65-3d515e0b1990");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "903be336-8422-4da4-b84e-1a877aa0816f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "d6b0bdad-dbd4-4458-a457-408f0917bbb5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "efdef68d-8c79-48af-b5c1-7dec37c9e108", "AQAAAAEAACcQAAAAENOCkuhDoXVasyOW/yiWYZsuHya4BHwrpK54HLN41ALoOUoRwqqMMJXGnMHiWejy9w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReport",
                table: "RecruitmentRatings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentRatingId",
                table: "ReasonReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentRatingId",
                table: "ImageReports",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "f47c2688-cd99-463a-9bc4-c46651ea3fd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "c3546493-4d60-4713-9924-5bd4e4bb0cb1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "9f118fc0-1b66-4c01-a4b7-9d4c984e6c41");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b5fafe76-713c-4345-9b71-125cc836b690", "AQAAAAEAACcQAAAAEGA5L0d8nJ+QNpC0+PQg9f5uNkizGbEEsHeXAdDpBof8rGMW65mQe7EXLM7hRJt/uQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_RecruitmentRatingId",
                table: "ReasonReports",
                column: "RecruitmentRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageReports_RecruitmentRatingId",
                table: "ImageReports",
                column: "RecruitmentRatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ImageReports",
                column: "RecruitmentRatingId",
                principalTable: "RecruitmentRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ReasonReports",
                column: "RecruitmentRatingId",
                principalTable: "RecruitmentRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
