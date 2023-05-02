using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplianceImages");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ApplianceImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplianceImage = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ApplianceID = table.Column<int>(nullable: false),
                    ApplianceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ApplianceImageId);
                    table.ForeignKey(
                        name: "FK_image",
                        column: x => x.ApplianceID,
                        principalTable: "Appliance",
                        principalColumn: "ApplianceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplianceID",
                table: "Images",
                column: "ApplianceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.CreateTable(
                name: "ApplianceImages",
                columns: table => new
                {
                    ApplianceImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplianceID = table.Column<int>(type: "int", nullable: false),
                    ApplianceId = table.Column<int>(type: "int", nullable: true),
                    ApplianceImage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplianceImages", x => x.ApplianceImageId);
                    table.ForeignKey(
                        name: "FK_image",
                        column: x => x.ApplianceID,
                        principalTable: "Appliance",
                        principalColumn: "ApplianceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplianceImages_ApplianceID",
                table: "ApplianceImages",
                column: "ApplianceID");
        }
    }
}
