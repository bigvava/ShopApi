using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductsConnectedToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AdderId",
                table: "Products",
                column: "AdderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_AdderId",
                table: "Products",
                column: "AdderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_AdderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AdderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdderId",
                table: "Products");
        }
    }
}
