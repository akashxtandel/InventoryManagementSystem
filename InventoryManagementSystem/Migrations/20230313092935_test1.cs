using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ApplianceID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ApplianceID",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ApplianceId",
                table: "Images",
                newName: "ApplianceID");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceID",
                table: "Images",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplianceID",
                table: "Images",
                column: "ApplianceID");

            migrationBuilder.AddForeignKey(
                name: "FK_image",
                table: "Images",
                column: "ApplianceID",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ApplianceID",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ApplianceID",
                table: "Images",
                newName: "ApplianceId");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ApplianceID",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplianceID",
                table: "Images",
                column: "ApplianceID");

            migrationBuilder.AddForeignKey(
                name: "FK_image",
                table: "Images",
                column: "ApplianceID",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
