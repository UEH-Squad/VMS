using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class addColumnActivityIdForTableFeedBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ActivityId",
                table: "Feedbacks",
                column: "ActivityId");

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

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Feedbacks");
        }
    }
}
