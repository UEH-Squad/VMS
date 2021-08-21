using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class ChangeRequirement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRequirements");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RequirementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityRequirements_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityRequirements_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Requirements",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 1, false, "Văn hay chữ tốt" });

            migrationBuilder.InsertData(
                table: "Requirements",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 2, false, "Bậc thầy tính toán" });

            migrationBuilder.InsertData(
                table: "Requirements",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 3, false, "Phù thủy hóa học" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityRequirements_ActivityId",
                table: "ActivityRequirements",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityRequirements_RequirementId",
                table: "ActivityRequirements",
                column: "RequirementId");
        }
    }
}
