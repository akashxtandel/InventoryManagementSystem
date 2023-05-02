using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class UpdateIssueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Issue__Appliance__4316F928",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "ApplianceID",
                table: "Issue",
                newName: "ApplianceId");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_ApplianceID",
                table: "Issue",
                newName: "IX_Issue_ApplianceId");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceId",
                table: "Issue",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Appliances",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Appliance_ApplianceId",
                table: "Issue",
                column: "ApplianceId",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Appliance_ApplianceId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "Appliances",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "ApplianceId",
                table: "Issue",
                newName: "ApplianceID");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_ApplianceId",
                table: "Issue",
                newName: "IX_Issue_ApplianceID");

            migrationBuilder.AlterColumn<int>(
                name: "ApplianceID",
                table: "Issue",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Issue__Appliance__4316F928",
                table: "Issue",
                column: "ApplianceID",
                principalTable: "Appliance",
                principalColumn: "ApplianceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
