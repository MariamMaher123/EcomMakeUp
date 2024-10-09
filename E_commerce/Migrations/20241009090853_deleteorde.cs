using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomMakeUp.Migrations
{
    public partial class deleteorde : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProducts_AspNetUsers_UserId",
                table: "ProductsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProducts_Products_ProductId",
                table: "ProductsProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProducts",
                table: "ProductsProducts");

            migrationBuilder.RenameTable(
                name: "ProductsProducts",
                newName: "Card");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProducts_UserId",
                table: "Card",
                newName: "IX_Card_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProducts_ProductId",
                table: "Card",
                newName: "IX_Card_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Card",
                table: "Card",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_AspNetUsers_UserId",
                table: "Card",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Products_ProductId",
                table: "Card",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_AspNetUsers_UserId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_Products_ProductId",
                table: "Card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Card",
                table: "Card");

            migrationBuilder.RenameTable(
                name: "Card",
                newName: "ProductsProducts");

            migrationBuilder.RenameIndex(
                name: "IX_Card_UserId",
                table: "ProductsProducts",
                newName: "IX_ProductsProducts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Card_ProductId",
                table: "ProductsProducts",
                newName: "IX_ProductsProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProducts",
                table: "ProductsProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProducts_AspNetUsers_UserId",
                table: "ProductsProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProducts_Products_ProductId",
                table: "ProductsProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
