using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class newtable13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appliance_ApplianceImages_ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "ImagesApplianceImageId",
                table: "Appliance");

            migrationBuilder.RenameColumn(
                name: "ApplianceID",
                table: "ApplianceImages",
                newName: "ApplianceId");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceId",
                table: "ApplianceImages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ApplianceID",
                table: "ApplianceImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApplianceImages_ApplianceID",
                table: "ApplianceImages",
                column: "ApplianceID");

            migrationBuilder.AddForeignKey(
                name: "FK_image",
                table: "ApplianceImages",
                column: "ApplianceID",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image",
                table: "ApplianceImages");

            migrationBuilder.DropIndex(
                name: "IX_ApplianceImages_ApplianceID",
                table: "ApplianceImages");

            migrationBuilder.DropColumn(
                name: "ApplianceID",
                table: "ApplianceImages");

            migrationBuilder.RenameColumn(
                name: "ApplianceId",
                table: "ApplianceImages",
                newName: "ApplianceID");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceID",
                table: "ApplianceImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImagesApplianceImageId",
                table: "Appliance",
                type: "int",
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
    }
}
