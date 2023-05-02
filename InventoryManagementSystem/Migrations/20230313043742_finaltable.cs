using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class finaltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplianceImageId",
                table: "Appliance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplianceImages",
                columns: table => new
                {
                    ApplianceImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplianceImage = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplianceImages", x => x.ApplianceImageId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__Images__398D8EEE",
                table: "Appliance");

            migrationBuilder.DropTable(
                name: "ApplianceImages");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_ApplianceImageId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "ApplianceImageId",
                table: "Appliance");
        }
    }
}
