using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class addnewcolumntouseimageandreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageReports_Feedbacks_FeedbackId",
                table: "ImageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId",
                table: "ReasonReports");

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentRatingId",
                table: "ReasonReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentRatingId",
                table: "ImageReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "c0abe7d7-4cc4-426a-89a4-7ffd4073e82a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "662b3c08-7293-4bc4-b8ce-5e5469917c73");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "2d4f4b57-2d5a-4c54-9c89-9f8b7e70d35b");

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_RecruitmentRatingId",
                table: "ReasonReports",
                column: "RecruitmentRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageReports_RecruitmentRatingId",
                table: "ImageReports",
                column: "RecruitmentRatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReports_Feedbacks_FeedbackId",
                table: "ImageReports",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ImageReports",
                column: "RecruitmentRatingId",
                principalTable: "RecruitmentRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId",
                table: "ReasonReports",
                column: "FeedbackId",
                principalTable: "Feedbacks",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageReports_Feedbacks_FeedbackId",
                table: "ImageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageReports_RecruitmentRatings_RecruitmentRatingId",
                table: "ImageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId",
                table: "ReasonReports");

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
                value: "1b0b3a03-5651-4879-aa4b-4970b51883fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "9cccb5d2-752c-4490-b7b2-5d4b336ca7be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "4ab96670-f459-40fd-b488-56ba6b7afb66");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReports_Feedbacks_FeedbackId",
                table: "ImageReports",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId",
                table: "ReasonReports",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
