using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class finaltableok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__Images__398D8EEE",
                table: "Appliance");

            migrationBuilder.AddForeignKey(
                name: "FK_Appliance_ApplianceImages_ApplianceImageId",
                table: "Appliance",
                column: "ApplianceImageId",
                principalTable: "ApplianceImages",
                principalColumn: "ApplianceImageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appliance_ApplianceImages_ApplianceImageId",
                table: "Appliance");

            migrationBuilder.AddForeignKey(
                name: "FK__Appliance__Images__398D8EEE",
                table: "Appliance",
                column: "ApplianceImageId",
                principalTable: "ApplianceImages",
                principalColumn: "ApplianceImageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
