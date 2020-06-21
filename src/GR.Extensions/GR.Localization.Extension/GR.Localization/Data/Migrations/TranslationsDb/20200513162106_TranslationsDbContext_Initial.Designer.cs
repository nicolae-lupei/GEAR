﻿// <auto-generated />
using System;
using GR.Localization.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GR.Localization.Data.Migrations.TranslationsDb
{
    [DbContext(typeof(TranslationsDbContext))]
    [Migration("20200513162106_TranslationsDbContext_Initial")]
    partial class TranslationsDbContext_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Localization")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GR.Localization.Abstractions.Models.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("Identifier");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("GR.Localization.Abstractions.Models.Translation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("GR.Localization.Abstractions.Models.TranslationItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Identifier")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("LanguageId");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("TranslationId");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("TranslationId");

                    b.ToTable("TranslationItems");
                });

            modelBuilder.Entity("GR.Localization.Abstractions.Models.TranslationItem", b =>
                {
                    b.HasOne("GR.Localization.Abstractions.Models.Language", "Language")
                        .WithMany("TranslationItems")
                        .HasForeignKey("LanguageId");

                    b.HasOne("GR.Localization.Abstractions.Models.Translation", "Translation")
                        .WithMany("Translations")
                        .HasForeignKey("TranslationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}