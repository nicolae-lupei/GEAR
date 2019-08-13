﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ST.Notifications.Data;

namespace ST.Notifications.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    partial class NotificationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Notifications")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ST.Audit.Models.TrackAudit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DatabaseContextName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("RecordId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("TrackEventType");

                    b.Property<string>("TypeFullName");

                    b.Property<string>("UserName");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("TrackAudits");
                });

            modelBuilder.Entity("ST.Audit.Models.TrackAuditDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("PropertyName");

                    b.Property<string>("PropertyType");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("TrackAuditId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TrackAuditId");

                    b.ToTable("TrackAuditDetails");
                });

            modelBuilder.Entity("ST.Notifications.Abstractions.Models.Data.NotificationEvent", b =>
                {
                    b.Property<string>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EventGroupName");

                    b.HasKey("EventId");

                    b.ToTable("NotificationEvents");
                });

            modelBuilder.Entity("ST.Notifications.Abstractions.Models.Data.NotificationSubscription", b =>
                {
                    b.Property<string>("NotificationEventId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("NotificationEventId", "RoleId");

                    b.ToTable("NotificationSubscriptions");
                });

            modelBuilder.Entity("ST.Notifications.Abstractions.Models.Data.NotificationTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("NotificationEventId")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("NotificationEventId");

                    b.ToTable("NotificationTemplates");
                });

            modelBuilder.Entity("ST.Audit.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("ST.Audit.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Notifications.Abstractions.Models.Data.NotificationSubscription", b =>
                {
                    b.HasOne("ST.Notifications.Abstractions.Models.Data.NotificationEvent", "NotificationEvent")
                        .WithMany()
                        .HasForeignKey("NotificationEventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Notifications.Abstractions.Models.Data.NotificationTemplate", b =>
                {
                    b.HasOne("ST.Notifications.Abstractions.Models.Data.NotificationEvent", "NotificationEvent")
                        .WithMany()
                        .HasForeignKey("NotificationEventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
