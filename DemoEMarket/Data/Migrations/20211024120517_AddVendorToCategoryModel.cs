using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoEMarket.Data.Migrations
{
    public partial class AddVendorToCategoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_VendorId",
                table: "Categories",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_VendorId",
                table: "Categories",
                column: "VendorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_VendorId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_VendorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Categories");
        }
    }
}
