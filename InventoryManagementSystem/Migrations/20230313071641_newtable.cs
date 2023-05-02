using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class newtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__Images__398D8EEE",
                table: "Appliance");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_ApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "ApplianceImageId",
                table: "Appliance");

            migrationBuilder.AddColumn<int>(
                name: "ApplianceID",
                table: "ApplianceImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplianceImage",
                table: "Appliance",
                type: "ApplianceImage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImagesApplianceImageId",
                table: "Appliance",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appliance_ImagesApplianceImageId",
                table: "Appliance",
                column: "ImagesApplianceImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appliance_ApplianceImages_ImagesApplianceImageId",
                table: "Appliance",
                column: "ImagesApplianceImageId",
                principalTable: "ApplianceImages",
                principalColumn: "ApplianceImageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appliance_ApplianceImages_ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "ApplianceID",
                table: "ApplianceImages");

            migrationBuilder.DropColumn(
                name: "ApplianceImage",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.AddColumn<int>(
                name: "ApplianceImageId",
                table: "Appliance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appliance_ApplianceImageId",
                table: "Appliance",
                column: "ApplianceImageId");

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
