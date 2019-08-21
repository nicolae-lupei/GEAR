﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ST.Entities.Data;

namespace ST.Entities.Migrations
{
    [DbContext(typeof(EntitiesDbContext))]
    partial class EntitiesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Entities")
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

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.EntityType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsSystem");

                    b.Property<string>("MachineName");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("EntityTypes");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldConfigValue", b =>
                {
                    b.Property<Guid>("TableModelFieldId");

                    b.Property<Guid>("TableFieldConfigId");

                    b.Property<string>("Value");

                    b.HasKey("TableModelFieldId", "TableFieldConfigId");

                    b.HasIndex("TableFieldConfigId");

                    b.ToTable("TableFieldConfigValues");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldConfigs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasColumnType("char(4)");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<Guid>("TableFieldTypeId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("TableFieldTypeId");

                    b.ToTable("TableFieldConfigs");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldGroups", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("GroupName");

                    b.HasKey("Id");

                    b.ToTable("TableFieldGroups");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasColumnType("char(2)");

                    b.Property<string>("DataType");

                    b.Property<string>("Name");

                    b.Property<Guid>("TableFieldGroupsId");

                    b.HasKey("Id");

                    b.HasIndex("TableFieldGroupsId");

                    b.ToTable("TableFieldTypes");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("EntityType");

                    b.Property<bool>("IsCommon");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPartOfDbContext");

                    b.Property<bool>("IsSystem");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("Table");
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableModelField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowNull");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DataType");

                    b.Property<string>("Description");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsCommon");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsSystem");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Synchronized");

                    b.Property<Guid>("TableFieldTypeId");

                    b.Property<Guid>("TableId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TableFieldTypeId");

                    b.HasIndex("TableId");

                    b.HasIndex("TenantId");

                    b.ToTable("TableFields");
                });

            modelBuilder.Entity("ST.Entities.Security.Abstractions.Models.EntityFieldPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ApplicationRoleId");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<int>("FieldAccessType");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("TableModelFieldId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationRoleId");

                    b.HasIndex("TableModelFieldId");

                    b.ToTable("EntityFieldPermissions");
                });

            modelBuilder.Entity("ST.Entities.Security.Abstractions.Models.EntityPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ApplicationRoleId");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("TableModelId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationRoleId");

                    b.HasIndex("TableModelId");

                    b.ToTable("EntityPermissions");
                });

            modelBuilder.Entity("ST.Entities.Security.Abstractions.Models.EntityPermissionAccess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessType");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("EntityPermissionId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("EntityPermissionId");

                    b.ToTable("EntityPermissionAccesses");
                });

            modelBuilder.Entity("ST.Audit.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("ST.Audit.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldConfigValue", b =>
                {
                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableFieldConfigs", "TableFieldConfig")
                        .WithMany()
                        .HasForeignKey("TableFieldConfigId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableModelField", "TableModelField")
                        .WithMany("TableFieldConfigValues")
                        .HasForeignKey("TableModelFieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldConfigs", b =>
                {
                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableFieldType", "TableFieldType")
                        .WithMany("TableFieldConfigs")
                        .HasForeignKey("TableFieldTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableFieldType", b =>
                {
                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableFieldGroups", "TableFieldGroups")
                        .WithMany("TableFieldTypes")
                        .HasForeignKey("TableFieldGroupsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Entities.Abstractions.Models.Tables.TableModelField", b =>
                {
                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableFieldType", "TableFieldType")
                        .WithMany()
                        .HasForeignKey("TableFieldTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ST.Entities.Abstractions.Models.Tables.TableModel", "Table")
                        .WithMany("TableFields")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.Entities.Security.Abstractions.Models.EntityPermissionAccess", b =>
                {
                    b.HasOne("ST.Entities.Security.Abstractions.Models.EntityPermission", "EntityPermission")
                        .WithMany("EntityPermissionAccesses")
                        .HasForeignKey("EntityPermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
