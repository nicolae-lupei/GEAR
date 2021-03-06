﻿// <auto-generated />
using System;
using GR.Identity.Groups.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GR.Identity.Groups.Infrastructure.Migrations
{
    [DbContext(typeof(GroupsDbContext))]
    [Migration("20200323145733_GroupsDbContext_Initial")]
    partial class GroupsDbContext_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Identity")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GR.Identity.Groups.Abstractions.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("GR.Identity.Groups.Abstractions.Models.GroupPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("GroupId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("PermissionCode")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupPermissions");
                });

            modelBuilder.Entity("GR.Identity.Groups.Abstractions.Models.UserGroup", b =>
                {
                    b.Property<Guid>("GroupId");

                    b.Property<Guid>("UserId");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("Id");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("TenantId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("GR.Identity.Groups.Abstractions.Models.GroupPermission", b =>
                {
                    b.HasOne("GR.Identity.Groups.Abstractions.Models.Group", "AuthGroup")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Identity.Groups.Abstractions.Models.UserGroup", b =>
                {
                    b.HasOne("GR.Identity.Groups.Abstractions.Models.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
