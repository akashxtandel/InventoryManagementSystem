using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplianceName",
                table: "Appliance");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Appliance",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryDetail",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDetail", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appliance_CategoryId",
                table: "Appliance",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Appliance__ApplianceCategory",
                table: "Appliance",
                column: "CategoryId",
                principalTable: "CategoryDetail",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__ApplianceCategory",
                table: "Appliance");

            migrationBuilder.DropTable(
                name: "CategoryDetail");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_CategoryId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Appliance");

            migrationBuilder.AddColumn<string>(
                name: "ApplianceName",
                table: "Appliance",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
