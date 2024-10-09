using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomMakeUp.Migrations
{
    public partial class deleteorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProducts_Orders_OrderId",
                table: "ProductsProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_ProductsProducts_OrderId",
                table: "ProductsProducts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductsProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdd",
                table: "ProductsProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductsProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ProductsProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProducts_UserId",
                table: "ProductsProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProducts_AspNetUsers_UserId",
                table: "ProductsProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProducts_AspNetUsers_UserId",
                table: "ProductsProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductsProducts_UserId",
                table: "ProductsProducts");

            migrationBuilder.DropColumn(
                name: "DateAdd",
                table: "ProductsProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductsProducts");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ProductsProducts");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductsProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProducts_OrderId",
                table: "ProductsProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProducts_Orders_OrderId",
                table: "ProductsProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
