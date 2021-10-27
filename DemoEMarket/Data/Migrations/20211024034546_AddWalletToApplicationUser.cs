using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoEMarket.Data.Migrations
{
    public partial class AddWalletToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Wallet",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wallet",
                table: "AspNetUsers");
        }
    }
}
