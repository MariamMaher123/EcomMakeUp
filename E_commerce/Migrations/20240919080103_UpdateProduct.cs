using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomMakeUp.Migrations
{
    public partial class UpdateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Available_Quan",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available_Quan",
                table: "Products");
        }
    }
}
