using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class RemoveReportByFromFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_ReportBy",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "ReportBy",
                table: "Feedbacks",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "IsProcessed",
                table: "Feedbacks",
                newName: "IsReportUser");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_ReportBy",
                table: "Feedbacks",
                newName: "IX_Feedbacks_UserId1");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "64922f05-22d2-4e89-aebb-2ef214e6c4ea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "5c3ee1ea-461c-48d1-95e2-9a75ea506db6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "98370b3b-585a-4b93-bd98-1eb9424163bf");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId1",
                table: "Feedbacks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId1",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Feedbacks",
                newName: "ReportBy");

            migrationBuilder.RenameColumn(
                name: "IsReportUser",
                table: "Feedbacks",
                newName: "IsProcessed");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_UserId1",
                table: "Feedbacks",
                newName: "IX_Feedbacks_ReportBy");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e570",
                column: "ConcurrencyStamp",
                value: "c2d941df-9de5-482e-8e33-ced9777c7ddb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e571",
                column: "ConcurrencyStamp",
                value: "557380c4-9cdf-4583-b3ca-276aeb0de3c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e572",
                column: "ConcurrencyStamp",
                value: "7c280274-5174-4f0f-a8cd-d62f65e57fa4");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Activities_ActivityId",
                table: "Feedbacks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_ReportBy",
                table: "Feedbacks",
                column: "ReportBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
