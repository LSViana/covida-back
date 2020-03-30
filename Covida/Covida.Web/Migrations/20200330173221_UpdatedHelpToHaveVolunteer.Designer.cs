﻿// <auto-generated />
using System;
using Covida.Data.Postgre;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Covida.Web.Migrations
{
    [DbContext(typeof(CovidaDbContext))]
    [Migration("20200330173221_UpdatedHelpToHaveVolunteer")]
    partial class UpdatedHelpToHaveVolunteer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Covida.Core.Domain.Help", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CancelledAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CancelledReason")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("HelpStatus")
                        .HasColumnType("integer");

                    b.Property<int?>("VolunteerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("VolunteerId");

                    b.ToTable("Helps");
                });

            modelBuilder.Entity("Covida.Core.Domain.HelpCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("HelpCategories");
                });

            modelBuilder.Entity("Covida.Core.Domain.HelpHasCategory", b =>
                {
                    b.Property<Guid>("HelpId")
                        .HasColumnType("uuid");

                    b.Property<int>("HelpCategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("HelpId", "HelpCategoryId");

                    b.HasIndex("HelpCategoryId");

                    b.ToTable("HelpHasCategories");
                });

            modelBuilder.Entity("Covida.Core.Domain.HelpItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<bool>("Complete")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("HelpId")
                        .HasColumnType("integer");

                    b.Property<Guid?>("HelpId1")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HelpId1");

                    b.ToTable("HelpItems");
                });

            modelBuilder.Entity("Covida.Core.Domain.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("HelpId")
                        .HasColumnType("uuid");

                    b.Property<int>("MessageStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HelpId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Covida.Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsVolunteer")
                        .HasColumnType("boolean");

                    b.Property<Point>("Location")
                        .HasColumnType("geometry");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Covida.Core.Domain.Help", b =>
                {
                    b.HasOne("Covida.Core.Domain.User", "Author")
                        .WithMany("Helps")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Covida.Core.Domain.User", "Volunteer")
                        .WithMany("Answers")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Covida.Core.Domain.HelpHasCategory", b =>
                {
                    b.HasOne("Covida.Core.Domain.HelpCategory", "HelpCategory")
                        .WithMany("CategoryHasHelps")
                        .HasForeignKey("HelpCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Covida.Core.Domain.Help", "Help")
                        .WithMany("HelpHasCategories")
                        .HasForeignKey("HelpId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Covida.Core.Domain.HelpItem", b =>
                {
                    b.HasOne("Covida.Core.Domain.Help", "Help")
                        .WithMany("HelpItems")
                        .HasForeignKey("HelpId1")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Covida.Core.Domain.Message", b =>
                {
                    b.HasOne("Covida.Core.Domain.Help", "Help")
                        .WithMany("Messages")
                        .HasForeignKey("HelpId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Covida.Core.Domain.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
