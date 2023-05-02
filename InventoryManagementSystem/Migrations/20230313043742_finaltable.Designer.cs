﻿// <auto-generated />
using System;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InventoryManagementSystem.Migrations
{
    [DbContext(typeof(InventoryManagementContext))]
    [Migration("20230313043742_finaltable")]
    partial class finaltable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InventoryManagementSystem.Models.Appliance", b =>
                {
                    b.Property<int>("ApplianceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ApplianceID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplianceCode")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("ApplianceImageId")
                        .HasColumnType("int");

                    b.Property<string>("ApplianceName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("IsActive")
                        .HasColumnName("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("IsAssign")
                        .HasColumnType("int");

                    b.Property<int>("IsDelete")
                        .HasColumnType("int");

                    b.Property<DateTime>("PruchaseDate")
                        .HasColumnType("date");

                    b.Property<int>("VendorId")
                        .HasColumnName("VendorID")
                        .HasColumnType("int");

                    b.Property<string>("WarrantPeriod")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("ApplianceId");

                    b.HasIndex("ApplianceImageId");

                    b.HasIndex("VendorId");

                    b.ToTable("Appliance");
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Images", b =>
                {
                    b.Property<int>("ApplianceImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ApplianceImageId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplianceImage")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("ApplianceImageId");

                    b.ToTable("ApplianceImages");
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Issue", b =>
                {
                    b.Property<int>("IssueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("IssueID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("AdminResponse")
                        .HasColumnName("AdminResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Appliances")
                        .HasColumnName("Appliances")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<int>("IsDelete")
                        .HasColumnType("int");

                    b.Property<string>("IssueDescription")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("Isview")
                        .HasColumnType("int");

                    b.Property<int>("ManagementId")
                        .HasColumnName("ManagementID")
                        .HasColumnType("int");

                    b.Property<string>("ResolveTime")
                        .HasColumnName("ResolveTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IssueId");

                    b.HasIndex("ManagementId");

                    b.ToTable("Issue");
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Management", b =>
                {
                    b.Property<int>("ManagementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ManagementID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignProjectName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("Camera")
                        .HasColumnName("camera")
                        .HasColumnType("int");

                    b.Property<int?>("Cpu")
                        .HasColumnName("cpu")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfIssue")
                        .HasColumnType("date");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("EmployEmailId")
                        .IsRequired()
                        .HasColumnName("EmployEmailID")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("Headphone")
                        .HasColumnName("headphone")
                        .HasColumnType("int");

                    b.Property<int>("IsDelete")
                        .HasColumnType("int");

                    b.Property<int?>("Keybord")
                        .HasColumnName("keyboard")
                        .HasColumnType("int");

                    b.Property<int?>("Monitor")
                        .HasColumnName("monitor")
                        .HasColumnType("int");

                    b.Property<int?>("Mouse")
                        .HasColumnName("mouse")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Role")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("ManagementId");

                    b.ToTable("Management");
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NotificationID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IsDelete")
                        .HasColumnType("int");

                    b.Property<int>("Isview")
                        .HasColumnType("int");

                    b.Property<int>("ManagementId")
                        .HasColumnName("ManagementID")
                        .HasColumnType("int");

                    b.Property<string>("Notificationn")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("NotificationId");

                    b.HasIndex("ManagementId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("InventoryManagementSystem.Vendor", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SupplierID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IsDelete")
                        .HasColumnType("int");

                    b.Property<string>("SupplierAddress")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SupplierNumber")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SupplierType")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("SupplierId")
                        .HasName("PK__Vendor__4BE666940F1B455A");

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Appliance", b =>
                {
                    b.HasOne("InventoryManagementSystem.Models.Images", "Images")
                        .WithMany("Appliance")
                        .HasForeignKey("ApplianceImageId")
                        .HasConstraintName("FK__Appliance__Images__398D8EEE")
                        .IsRequired();

                    b.HasOne("InventoryManagementSystem.Vendor", "Vendor")
                        .WithMany("Appliance")
                        .HasForeignKey("VendorId")
                        .HasConstraintName("FK__Appliance__Vendo__398D8EEE")
                        .IsRequired();
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Issue", b =>
                {
                    b.HasOne("InventoryManagementSystem.Models.Management", "Management")
                        .WithMany("Issue")
                        .HasForeignKey("ManagementId")
                        .HasConstraintName("FK__Issue__Managemen__4222D4EF")
                        .IsRequired();
                });

            modelBuilder.Entity("InventoryManagementSystem.Models.Notification", b =>
                {
                    b.HasOne("InventoryManagementSystem.Models.Management", "Management")
                        .WithMany("Notification")
                        .HasForeignKey("ManagementId")
                        .HasConstraintName("FK__Notificat__Manag__3F466844")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
