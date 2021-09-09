using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class _AddActivityTargetTableAndReasonReportTableAndImageReportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Commission",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTargets_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageReports_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReasonReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReasonReports_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ActivityId",
                table: "Feedbacks",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTargets_ActivityId",
                table: "ActivityTargets",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageReports_FeedbackId",
                table: "ImageReports",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_ReasonReports_FeedbackId",
                table: "ReasonReports",
                column: "FeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropTable(
                name: "ActivityTargets");

            migrationBuilder.DropTable(
                name: "ImageReports");

            migrationBuilder.DropTable(
                name: "ReasonReports");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Commission",
                table: "Activities");
        }
    }
}
