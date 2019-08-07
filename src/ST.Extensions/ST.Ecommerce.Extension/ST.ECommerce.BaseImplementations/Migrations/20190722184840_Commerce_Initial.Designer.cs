﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ST.ECommerce.BaseImplementations.Data;

namespace ST.ECommerce.BaseImplementations.Migrations
{
    [DbContext(typeof(CommerceDbContext))]
    [Migration("20190722184840_Commerce_Initial")]
    partial class Commerce_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Commerce")
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

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Address.Country", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code3")
                        .IsRequired();

                    b.Property<bool>("IsBillingEnabled");

                    b.Property<bool>("IsCityEnabled");

                    b.Property<bool>("IsDistrictEnabled");

                    b.Property<bool>("IsShippingEnabled");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Address.StatesOrProvinces", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("CountryId")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("StatesOrProvinces");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.AttributeGroup", b =>
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

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("AttributeGroup");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<decimal>("Percentage");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("OrderState");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.OrderHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("OrderId");

                    b.Property<int>("OrderState");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderHistory");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<Guid>("BrandId");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("ProductTypeId");

                    b.Property<Guid?>("TenantId");

                    b.Property<byte[]>("Thumbnail");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("Name");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AttributeGroupId");

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("ProductId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("AttributeGroupId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAttribute");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductCategory", b =>
                {
                    b.Property<Guid>("CategoryId");

                    b.Property<Guid>("ProductId");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductDiscount", b =>
                {
                    b.Property<Guid>("DiscountId");

                    b.Property<Guid>("ProductId");

                    b.HasKey("DiscountId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDiscounts");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("ProductId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductOptions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("ProductId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOptions");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductOrder", b =>
                {
                    b.Property<Guid>("OrderId");

                    b.Property<Guid>("ProductId");

                    b.Property<double>("Price");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOrders");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductPrice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<double>("Price");

                    b.Property<Guid>("ProductId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ShipmentAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<string>("CountryId")
                        .IsRequired();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Street");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("ShipmentAddresses");
                });

            modelBuilder.Entity("ST.Audit.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("ST.Audit.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Address.StatesOrProvinces", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Address.Country", "Country")
                        .WithMany("StatesOrProvinces")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.OrderHistory", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Order", "Order")
                        .WithMany("OrderHistories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.Product", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.ECommerce.Abstractions.Models.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductAttribute", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.AttributeGroup", "AttributeGroup")
                        .WithMany()
                        .HasForeignKey("AttributeGroupId");

                    b.HasOne("ST.ECommerce.Abstractions.Models.Product")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductCategory", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductDiscount", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductImage", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductOptions", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductOrder", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Order", "Order")
                        .WithMany("ProductOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ProductPrice", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Product", "Product")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ST.ECommerce.Abstractions.Models.ShipmentAddress", b =>
                {
                    b.HasOne("ST.ECommerce.Abstractions.Models.Address.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
