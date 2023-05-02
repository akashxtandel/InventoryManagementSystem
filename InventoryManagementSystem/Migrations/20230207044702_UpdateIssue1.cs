using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class UpdateIssue1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Appliance_ApplianceId",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_ApplianceId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ApplianceId",
                table: "Issue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplianceId",
                table: "Issue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ApplianceId",
                table: "Issue",
                column: "ApplianceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Appliance_ApplianceId",
                table: "Issue",
                column: "ApplianceId",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
