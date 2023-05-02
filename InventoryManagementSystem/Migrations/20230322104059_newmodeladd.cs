using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class newmodeladd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Appliance");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Appliance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appliance_BrandId",
                table: "Appliance",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appliance__BrandName",
                table: "Appliance");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropIndex(
                name: "IX_Appliance_BrandId",
                table: "Appliance");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Appliance");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Appliance",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
