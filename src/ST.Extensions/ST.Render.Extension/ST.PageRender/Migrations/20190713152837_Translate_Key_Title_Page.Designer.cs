﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ST.PageRender.Data;

namespace ST.PageRender.Migrations
{
    [DbContext(typeof(DynamicPagesDbContext))]
    [Migration("20190713152837_Translate_Key_Title_Page")]
    partial class Translate_Key_Title_Page
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Pages")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
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

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.Block", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<Guid>("BlockCategoryId");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("CssCode");

                    b.Property<string>("Description");

                    b.Property<string>("FaIcon");

                    b.Property<string>("HtmlCode");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TableModelId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("BlockCategoryId");

                    b.HasIndex("TenantId");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.BlockAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<Guid>("BlockId");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DefaultValue");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("BlockId");

                    b.ToTable("BlockAttributes");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.BlockCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("BlockCategories");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEnabledAcl");

                    b.Property<bool>("IsLayout");

                    b.Property<bool>("IsSystem");

                    b.Property<Guid?>("LayoutId");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("PageTypeId");

                    b.Property<string>("Path");

                    b.Property<Guid?>("SettingsId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.HasIndex("PageTypeId");

                    b.HasIndex("SettingsId");

                    b.HasIndex("TenantId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageScript", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("Order");

                    b.Property<Guid>("PageId");

                    b.Property<string>("Script");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.ToTable("PageScripts");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("CssCode");

                    b.Property<string>("Description");

                    b.Property<string>("HtmlCode");

                    b.Property<string>("Icon");

                    b.Property<string>("Identifier");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("JsCode");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Title");

                    b.Property<string>("TitleTranslateKey");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("PageSettings");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageStyle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("Order");

                    b.Property<Guid>("PageId");

                    b.Property<string>("Script");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.ToTable("PageStyles");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsSystem");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("PageTypes");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.PagesACL.RolePagesAcl", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("PageId");

                    b.Property<bool>("AllowAccess");

                    b.HasKey("RoleId", "PageId");

                    b.HasIndex("PageId");

                    b.ToTable("RolePagesAcls");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.RenderTemplates.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("IdentifierName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModel", b =>
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

                    b.Property<Guid>("TableModelId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("ViewModels");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFieldCode", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Code");

                    b.ToTable("ViewModelFieldCodesCodes");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFieldConfiguration", b =>
                {
                    b.Property<int>("ViewModelFieldCodeId");

                    b.Property<Guid>("ViewModelFieldId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("ViewModelFieldCodeId", "ViewModelFieldId");

                    b.HasIndex("ViewModelFieldId");

                    b.ToTable("ViewModelFieldConfigurations");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFields", b =>
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

                    b.Property<Guid?>("TableModelFieldsId");

                    b.Property<string>("Template");

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Translate");

                    b.Property<int>("Version");

                    b.Property<Guid>("ViewModelId");

                    b.Property<int>("VirtualDataType");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("ViewModelId");

                    b.ToTable("ViewModelFields");
                });

            modelBuilder.Entity("ST.Audit.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("ST.Audit.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.Block", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.BlockCategory", "BlockCategory")
                        .WithMany()
                        .HasForeignKey("BlockCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.BlockAttribute", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.Block")
                        .WithMany("Attributes")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.Page", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.Page", "Layout")
                        .WithMany()
                        .HasForeignKey("LayoutId");

                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.PageType", "PageType")
                        .WithMany()
                        .HasForeignKey("PageTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.PageSettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId");
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageScript", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.Page")
                        .WithMany("PageScripts")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.Pages.PageStyle", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.Page")
                        .WithMany("PageStyles")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.PagesACL.RolePagesAcl", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.Pages.Page", "Page")
                        .WithMany("RolePagesAcls")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFieldConfiguration", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFieldCode", "ViewModelFieldCode")
                        .WithMany()
                        .HasForeignKey("ViewModelFieldCodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFields", "ViewModelField")
                        .WithMany("Configurations")
                        .HasForeignKey("ViewModelFieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.PageRender.Abstractions.Models.ViewModels.ViewModelFields", b =>
                {
                    b.HasOne("ST.PageRender.Abstractions.Models.ViewModels.ViewModel", "ViewModel")
                        .WithMany("ViewModelFields")
                        .HasForeignKey("ViewModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
