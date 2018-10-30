using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class serviceschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ServiceItems",
                newName: "HourPrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "UnitDuration",
                table: "ServiceItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitDuration",
                table: "ServiceItems");

            migrationBuilder.RenameColumn(
                name: "HourPrice",
                table: "ServiceItems",
                newName: "Price");
        }
    }
}
