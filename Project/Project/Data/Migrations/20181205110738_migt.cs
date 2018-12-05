using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class migt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MmInventoryItemCategories_InventoryItems_InventoryItemId",
                table: "MmInventoryItemCategories");

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemCategoryId",
                table: "MmInventoryItemCategories",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_MmInventoryItemCategories_InventoryItemCategoryId",
                table: "MmInventoryItemCategories",
                column: "InventoryItemCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MmInventoryItemCategories_InventoryItemCategories_InventoryItemCategoryId",
                table: "MmInventoryItemCategories",
                column: "InventoryItemCategoryId",
                principalTable: "InventoryItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MmInventoryItemCategories_InventoryItemCategories_InventoryItemCategoryId",
                table: "MmInventoryItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_MmInventoryItemCategories_InventoryItemCategoryId",
                table: "MmInventoryItemCategories");

            migrationBuilder.DropColumn(
                name: "InventoryItemCategoryId",
                table: "MmInventoryItemCategories");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddForeignKey(
                name: "FK_MmInventoryItemCategories_InventoryItems_InventoryItemId",
                table: "MmInventoryItemCategories",
                column: "InventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
