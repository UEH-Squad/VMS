using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class _AddFeedbackId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_Activities_ActivityId",
                table: "ReasonReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_AspNetUsers_UserId",
                table: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_ReasonReports_ActivityId",
                table: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_ReasonReports_UserId",
                table: "ReasonReports");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "ReasonReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReasonReports");

            migrationBuilder.AddColumn<string>(
                name: "FeedbackId",
                table: "ReasonReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId1",
                table: "ReasonReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_FeedbackId1",
                table: "ReasonReports",
                column: "FeedbackId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId1",
                table: "ReasonReports",
                column: "FeedbackId1",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReasonReports_Feedbacks_FeedbackId1",
                table: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_ReasonReports_FeedbackId1",
                table: "ReasonReports");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "ReasonReports");

            migrationBuilder.DropColumn(
                name: "FeedbackId1",
                table: "ReasonReports");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "ReasonReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReasonReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_ActivityId",
                table: "ReasonReports",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_UserId",
                table: "ReasonReports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_Activities_ActivityId",
                table: "ReasonReports",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReasonReports_AspNetUsers_UserId",
                table: "ReasonReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
