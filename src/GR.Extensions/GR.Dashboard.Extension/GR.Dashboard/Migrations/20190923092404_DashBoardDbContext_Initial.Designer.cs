﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using GR.Dashboard.Data;

namespace GR.Dashboard.Migrations
{
    [DbContext(typeof(DashBoardDbContext))]
    [Migration("20190923092404_DashBoardDbContext_Initial")]
    partial class DashBoardDbContext_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DashBoard")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAudit", b =>
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

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAuditDetails", b =>
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

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.CustomWidget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCache");

                    b.Property<string>("Author");

                    b.Property<string>("BackGroundColor");

                    b.Property<int>("BorderRadius");

                    b.Property<string>("BorderStyle");

                    b.Property<TimeSpan>("CacheRefreshSpan");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("ClassAttribute");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RowId");

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid>("WidgetGroupId");

                    b.Property<int>("WidgetTemplateType");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RowId");

                    b.HasIndex("WidgetGroupId");

                    b.ToTable("CustomWidgets");
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.Row", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Rows");
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("WidgetGroups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7fd7f268-db82-4b9c-be2a-1e8c271739c2"),
                            Changed = new DateTime(2019, 9, 23, 9, 24, 4, 383, DateTimeKind.Utc).AddTicks(3401),
                            Created = new DateTime(2019, 9, 23, 9, 24, 4, 383, DateTimeKind.Utc).AddTicks(2439),
                            IsDeleted = false,
                            Name = "Charts",
                            Order = 1,
                            Version = 0
                        },
                        new
                        {
                            Id = new Guid("4dfecf64-7dfb-43ed-9bce-2de01544e5f7"),
                            Changed = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2585),
                            Created = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2576),
                            IsDeleted = false,
                            Name = "Reports",
                            Order = 2,
                            Version = 0
                        },
                        new
                        {
                            Id = new Guid("fd05e6c8-ec04-4b26-aec7-32057055477a"),
                            Changed = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2919),
                            Created = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2918),
                            IsDeleted = false,
                            Name = "Custom",
                            Order = 3,
                            Version = 0
                        },
                        new
                        {
                            Id = new Guid("e177db49-c0af-4355-bd82-02ab0224d822"),
                            Changed = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2956),
                            Created = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(2955),
                            IsDeleted = false,
                            Name = "Samples",
                            Order = 4,
                            Version = 0
                        },
                        new
                        {
                            Id = new Guid("1b5ff978-6ab0-4ba7-9084-577c525c9adf"),
                            Changed = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(3027),
                            Created = new DateTime(2019, 9, 23, 9, 24, 4, 411, DateTimeKind.Utc).AddTicks(3025),
                            IsDeleted = false,
                            Name = "Custom",
                            Order = 5,
                            Version = 0
                        });
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ChartWidget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCache");

                    b.Property<string>("Author");

                    b.Property<string>("BackGroundColor");

                    b.Property<int>("BorderRadius");

                    b.Property<string>("BorderStyle");

                    b.Property<TimeSpan>("CacheRefreshSpan");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("ClassAttribute");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RowId");

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid>("WidgetGroupId");

                    b.Property<int>("WidgetTemplateType");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RowId");

                    b.HasIndex("WidgetGroupId");

                    b.ToTable("ChartWidgets");
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ListWidget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCache");

                    b.Property<string>("Author");

                    b.Property<string>("BackGroundColor");

                    b.Property<int>("BorderRadius");

                    b.Property<string>("BorderStyle");

                    b.Property<TimeSpan>("CacheRefreshSpan");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("ClassAttribute");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RowId");

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid>("WidgetGroupId");

                    b.Property<int>("WidgetTemplateType");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RowId");

                    b.HasIndex("WidgetGroupId");

                    b.ToTable("ListWidgets");
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ReportWidget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCache");

                    b.Property<string>("Author");

                    b.Property<string>("BackGroundColor");

                    b.Property<int>("BorderRadius");

                    b.Property<string>("BorderStyle");

                    b.Property<TimeSpan>("CacheRefreshSpan");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("ClassAttribute");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RowId");

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid>("WidgetGroupId");

                    b.Property<int>("WidgetTemplateType");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RowId");

                    b.HasIndex("WidgetGroupId");

                    b.ToTable("ReportWidgets");
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.TabbedWidget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCache");

                    b.Property<string>("Author");

                    b.Property<string>("BackGroundColor");

                    b.Property<int>("BorderRadius");

                    b.Property<string>("BorderStyle");

                    b.Property<TimeSpan>("CacheRefreshSpan");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("ClassAttribute");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RowId");

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid>("WidgetGroupId");

                    b.Property<int>("WidgetTemplateType");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RowId");

                    b.HasIndex("WidgetGroupId");

                    b.ToTable("TabbedWidgets");
                });

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("GR.Audit.Abstractions.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.CustomWidget", b =>
                {
                    b.HasOne("GR.Dashboard.Abstractions.Models.Row", "Row")
                        .WithMany()
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Dashboard.Abstractions.Models.WidgetGroup", "WidgetGroup")
                        .WithMany()
                        .HasForeignKey("WidgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ChartWidget", b =>
                {
                    b.HasOne("GR.Dashboard.Abstractions.Models.Row", "Row")
                        .WithMany("ChartWidgets")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Dashboard.Abstractions.Models.WidgetGroup", "WidgetGroup")
                        .WithMany()
                        .HasForeignKey("WidgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ListWidget", b =>
                {
                    b.HasOne("GR.Dashboard.Abstractions.Models.Row", "Row")
                        .WithMany("ListWidgets")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Dashboard.Abstractions.Models.WidgetGroup", "WidgetGroup")
                        .WithMany()
                        .HasForeignKey("WidgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.ReportWidget", b =>
                {
                    b.HasOne("GR.Dashboard.Abstractions.Models.Row", "Row")
                        .WithMany("ReportWidgets")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Dashboard.Abstractions.Models.WidgetGroup", "WidgetGroup")
                        .WithMany()
                        .HasForeignKey("WidgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Dashboard.Abstractions.Models.WidgetTypes.TabbedWidget", b =>
                {
                    b.HasOne("GR.Dashboard.Abstractions.Models.Row", "Row")
                        .WithMany("TabbedWidgets")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Dashboard.Abstractions.Models.WidgetGroup", "WidgetGroup")
                        .WithMany()
                        .HasForeignKey("WidgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}