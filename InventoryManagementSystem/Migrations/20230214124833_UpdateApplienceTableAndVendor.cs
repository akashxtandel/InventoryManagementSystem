using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class UpdateApplienceTableAndVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierAddress",
                table: "Vendor",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierType",
                table: "Vendor",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Management",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValueSql: "('employee')");

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Appliance",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierAddress",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "SupplierType",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Appliance");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Management",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                defaultValueSql: "('employee')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
