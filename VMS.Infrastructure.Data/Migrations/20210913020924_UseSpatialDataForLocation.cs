using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class UseSpatialDataForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Activities",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Activities");
        }
    }
}
