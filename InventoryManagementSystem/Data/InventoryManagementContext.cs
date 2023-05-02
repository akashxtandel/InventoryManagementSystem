using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data
{
    public class InventoryManagementContext: DbContext
    {
        public InventoryManagementContext()
        {
        }

        public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appliance> Appliance { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<Management> Management { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<BrandDetail> BrandDetail { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetail { get; set; }
        public virtual DbSet<OthersAppliance> Other_Applince { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appliance>(entity =>
            {
                entity.Property(e => e.ApplianceId).HasColumnName("ApplianceID");

                entity.Property(e => e.ApplianceCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId).HasColumnName("BrandId");

                entity.Property(e => e.ApplianceImage).HasColumnType("ApplianceImage");

                entity.Property(e => e.PruchaseDate).HasColumnType("date");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");

                entity.Property(e => e.WarrantPeriod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("IsActive");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Appliance)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appliance__Vendo__398D8EEE");

                entity.HasOne(d => d.Brand)
                   .WithMany(p => p.Appliance)
                   .HasForeignKey(d => d.BrandId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK__Appliance__BrandName");

                entity.HasOne(d => d.ApplianceCategory)
                   .WithMany(p => p.Appliance)
                   .HasForeignKey(d => d.CategoryId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK__Appliance__ApplianceCategory");

            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.Property(e => e.IssueId).HasColumnName("IssueID");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Appliances).HasColumnName("Appliances");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Time_Date).HasColumnType("datetime");

                entity.Property(e => e.Time_Date).HasColumnName("Time_Date");

                entity.Property(e => e.Time_Only).HasColumnName("Time_Only");
                
                entity.Property(e => e.IssueDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AdminResponse).HasColumnName("AdminResponse");
                
                entity.Property(e => e.ResolveTime).HasColumnName("ResolveTime");

                entity.Property(e => e.ManagementId).HasColumnName("ManagementID");

                entity.HasOne(d => d.Management)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.ManagementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__Managemen__4222D4EF");

            });

            modelBuilder.Entity<Management>(entity =>
            {
                entity.Property(e => e.ManagementId).HasColumnName("ManagementID");

                entity.Property(e => e.AssignProjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Camera).HasColumnName("camera");

                entity.Property(e => e.Cpu).HasColumnName("cpu");

                entity.Property(e => e.DateOfIssue).HasColumnType("date");

                entity.Property(e => e.Designation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployEmailId)
                    .IsRequired()
                    .HasColumnName("EmployEmailID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Headphone).HasColumnName("headphone");

                entity.Property(e => e.Keybord).HasColumnName("keyboard");

                entity.Property(e => e.Monitor).HasColumnName("monitor");

                entity.Property(e => e.Mouse).HasColumnName("mouse");

                entity.Property(e => e.Others).HasColumnName("others");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.ManagementId).HasColumnName("ManagementID");

                entity.Property(e => e.Notificationn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Management)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.ManagementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Manag__3F466844");

            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.SupplierId)
                    .HasName("PK__Vendor__4BE666940F1B455A");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SupplierType)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.SupplierAddress)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false);

            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.ApplianceImageId).HasColumnName("ApplianceImageId");

                entity.Property(e => e.ApplianceImage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Appliance)
                   .WithMany(p => p.Images)
                   .HasForeignKey(d => d.ApplianceID)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_image");

            });

            modelBuilder.Entity<BrandDetail>(entity =>
            {
                entity.Property(e => e.BrandId).HasColumnName("BrandId");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
               
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");

                entity.Property(e => e.IsOthers).HasColumnName("IsOthers");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<OthersAppliance>(entity =>
            {
                entity.Property(e => e.Other_id).HasColumnName("Other_id");

                entity.HasOne(d => d.Appliance)
                   .WithMany(p => p.OthersAppliances)
                   .HasForeignKey(d => d.Appliance_id)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_applianceid");

                entity.HasOne(d => d.Management)
                  .WithMany(p => p.OthersAppliances)
                  .HasForeignKey(d => d.Management_id)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_managementid");
            });
        }
    }
}
