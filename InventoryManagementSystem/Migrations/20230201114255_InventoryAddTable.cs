using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class InventoryAddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Management",
                columns: table => new
                {
                    ManagementID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    EmployEmailID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Role = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValueSql: "('employee')"),
                    Designation = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    AssignProjectName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "date", nullable: false),
                    monitor = table.Column<int>(nullable: false),
                    cpu = table.Column<int>(nullable: false),
                    keybord = table.Column<int>(nullable: false),
                    mouse = table.Column<int>(nullable: false),
                    headphone = table.Column<int>(nullable: false),
                    camera = table.Column<int>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Management", x => x.ManagementID);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    SupplierID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SupplierNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vendor__4BE666940F1B455A", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Notificationn = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ManagementID = table.Column<int>(nullable: false),
                    Isview = table.Column<int>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK__Notificat__Manag__3F466844",
                        column: x => x.ManagementID,
                        principalTable: "Management",
                        principalColumn: "ManagementID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appliance",
                columns: table => new
                {
                    ApplianceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplianceName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ApplianceCode = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    BrandName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    PruchaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    WarrantPeriod = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    VendorID = table.Column<int>(nullable: false),
                    IsAssign = table.Column<int>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appliance", x => x.ApplianceID);
                    table.ForeignKey(
                        name: "FK__Appliance__Vendo__398D8EEE",
                        column: x => x.VendorID,
                        principalTable: "Vendor",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IssueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagementID = table.Column<int>(nullable: false),
                    ApplianceID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    AssignedProjectName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IssueDescription = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Action = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Isview = table.Column<int>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.IssueID);
                    table.ForeignKey(
                        name: "FK__Issue__Appliance__4316F928",
                        column: x => x.ApplianceID,
                        principalTable: "Appliance",
                        principalColumn: "ApplianceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Issue__Managemen__4222D4EF",
                        column: x => x.ManagementID,
                        principalTable: "Management",
                        principalColumn: "ManagementID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appliance_VendorID",
                table: "Appliance",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ApplianceID",
                table: "Issue",
                column: "ApplianceID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ManagementID",
                table: "Issue",
                column: "ManagementID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ManagementID",
                table: "Notification",
                column: "ManagementID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Appliance");

            migrationBuilder.DropTable(
                name: "Management");

            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}
