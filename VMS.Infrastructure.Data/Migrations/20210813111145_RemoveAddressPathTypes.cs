using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class RemoveAddressPathTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressPaths_AddressPathTypes_AddressPathTypeId",
                table: "AddressPaths");

            migrationBuilder.DropTable(
                name: "AddressPathTypes");

            migrationBuilder.DropIndex(
                name: "IX_AddressPaths_AddressPathTypeId",
                table: "AddressPaths");

            migrationBuilder.RenameColumn(
                name: "AddressPathTypeId",
                table: "AddressPaths",
                newName: "Depth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "AddressPaths",
                newName: "AddressPathTypeId");

            migrationBuilder.CreateTable(
                name: "AddressPathTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressPathTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressPaths_AddressPathTypeId",
                table: "AddressPaths",
                column: "AddressPathTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressPaths_AddressPathTypes_AddressPathTypeId",
                table: "AddressPaths",
                column: "AddressPathTypeId",
                principalTable: "AddressPathTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
