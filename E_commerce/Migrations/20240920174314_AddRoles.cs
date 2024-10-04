﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomMakeUp.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"AspNetRoles",
                columns: new[] {"Id" ,"Name" , "NormalizedName" , "ConcurrencyStamp"},
                values:new object[] {Guid.NewGuid().ToString() , "User", "User".ToUpper() , Guid.NewGuid().ToString()}
                
                );

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }

               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [AspNetRoles]");
        }
    }
}
