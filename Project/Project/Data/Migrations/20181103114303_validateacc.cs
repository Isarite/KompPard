using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class validateacc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSubscribed",
                table: "AspNetUsers",
                newName: "Subscribed");

            migrationBuilder.AddColumn<bool>(
                name: "AccountValidated",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountValidated",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Subscribed",
                table: "AspNetUsers",
                newName: "IsSubscribed");
        }
    }
}
