using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddNewFieldInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "keybord",
                table: "Management",
                newName: "keyboard");

            migrationBuilder.AddColumn<string>(
                name: "AdminResponse",
                table: "Issue",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminResponse",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "keyboard",
                table: "Management",
                newName: "keybord");
        }
    }
}
