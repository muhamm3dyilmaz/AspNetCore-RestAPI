using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreSample.Migrations
{
    public partial class categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1cb20c38-ec99-46ea-953a-88285b79e646");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "352957c3-0056-47d1-93e8-c8fc684f957e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7338fe1a-e95c-408f-aee8-3787055c218d");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ecb2c45-468a-4f33-93b2-014b2d8b23ab", "d63db691-7ff0-411a-b7a1-b7b332775ecb", "User", "USER" },
                    { "7a2e0623-fb53-4872-bc90-f31e41ddf268", "2a5c1c0f-9242-4f47-b424-b4e14bb4e717", "Editor", "EDITOR" },
                    { "af5f5359-e908-4796-887e-78cadf797da3", "aaf8d7de-1ed2-4f75-b9f3-34276dea59ca", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Gündem" },
                    { 2, "Spor" },
                    { 3, "Finans" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ecb2c45-468a-4f33-93b2-014b2d8b23ab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a2e0623-fb53-4872-bc90-f31e41ddf268");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af5f5359-e908-4796-887e-78cadf797da3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1cb20c38-ec99-46ea-953a-88285b79e646", "cce481e5-681e-492e-892e-7ecc88b42f48", "Editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "352957c3-0056-47d1-93e8-c8fc684f957e", "b4efb11d-9710-4489-a977-3f09b491bf34", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7338fe1a-e95c-408f-aee8-3787055c218d", "66c68453-0798-4481-b1ea-8205b72a36c5", "User", "USER" });
        }
    }
}
