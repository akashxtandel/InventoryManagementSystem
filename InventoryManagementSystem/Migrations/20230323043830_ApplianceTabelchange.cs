using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class ApplianceTabelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.CreateTable(
                name: "BrandDetail",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandDetail", x => x.BrandId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance",
                column: "BrandId",
                principalTable: "BrandDetail",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance");

            migrationBuilder.DropTable(
                name: "BrandDetail");

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsDelete = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
