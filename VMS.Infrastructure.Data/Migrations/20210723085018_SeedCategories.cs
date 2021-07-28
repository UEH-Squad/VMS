using Microsoft.EntityFrameworkCore.Migrations;

namespace VMS.Infrastructure.Data.Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "769477b1-da6f-4911-a251-65111c03b7b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7211",
                column: "ConcurrencyStamp",
                value: "590f2b75-39cf-4774-90f7-c0393ac36425");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7212",
                column: "ConcurrencyStamp",
                value: "6522de6d-3160-4d55-b9b8-48db7b89b53b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26ab2c19-cf74-4701-b2b1-8f5a03820750", "AQAAAAEAACcQAAAAEO6qC0p7WvsYzdF/PwGiAdb554AoWI7PzDYDq22kEsQuZoM8O7wQeiDk48NtKCtrhw==", "4eeee987-b542-43ec-9aed-45d9f801cbe6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "21031254-b577-4ad2-bd0f-581183f443ce", "AQAAAAEAACcQAAAAENzLpelIXMYbt1Z4ufsGINcvUDIIQwOkY//zhoZ1FNreWbhrg3SB/W/qSGnXu4LOoA==", "9622d1b3-16e3-41c8-bf6d-70b8cb4f340f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69f2428f-a84b-4a46-a7dc-2bfcc7bbe781", "AQAAAAEAACcQAAAAECUPCVw7ax7MApV5sNhINgJqRAOyt4uMtMgZ4ilVzaV45WpwXaeMlU2xX3t/u+lY5Q==", "dd249bcf-1b12-4365-8d91-04881d0e1278" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "Test category 1" },
                    { 2, false, "Test category 2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "4ca5ff4b-5cfa-45f1-b8a5-7c839091d29d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7211",
                column: "ConcurrencyStamp",
                value: "d9cf6aba-f4df-4771-a2fd-25c047250d4e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7212",
                column: "ConcurrencyStamp",
                value: "a7f43ae9-0a81-4105-b6a1-1b09ab9daee9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75f50c50-e3bd-4f58-9883-ee4d7cac7f60", "AQAAAAEAACcQAAAAEIv1I7HMM5tV5yNc7X49/ZibSWqqWgs5kjjjcL4jz/LaScIPUWvp4BcRdWAeABcawg==", "47507e9c-abd7-41ed-b8cb-f06a872ff0e9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5584e31-8e77-43ba-b544-d8bb647e2da7", "AQAAAAEAACcQAAAAEONp1aG8NQt2z7MhVXbntiCs06NkXvCITo3wzLei2sOvx3mT1+1+WWjdqR3gB8I5+w==", "a11a70f9-1d93-4f1f-9b08-7f5947a8e275" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "723b34f3-6172-4942-bafd-8caa08393923", "AQAAAAEAACcQAAAAEIx+crfvcGShINOpB5laEXO+aos5Jx91Z8rWTZbY+A6eT/rT+HG7WdaxYbkmRRJI4Q==", "dc0c0ef8-df4a-4a84-9e3a-be7322041907" });
        }
    }
}
