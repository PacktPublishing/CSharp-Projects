﻿// <auto-generated />
using CardTrackerWebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    [DbContext(typeof(CardTrackerDbContext))]
    [Migration("20250316033523_SeedAdminUser")]
    partial class SeedAdminUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("CardTrackerWebApi.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAdmin = true,
                            PasswordHash = new byte[] { 157, 181, 200, 155, 137, 142, 235, 58, 87, 195, 100, 51, 33, 84, 250, 248, 219, 174, 47, 125, 171, 145, 189, 73, 241, 83, 221, 193, 51, 119, 209, 203 },
                            Salt = new byte[] { 65, 100, 109, 105, 110, 85, 115, 101, 114, 85, 115, 101, 115, 65, 72, 97, 114, 100, 99, 111, 100, 101, 100, 83, 97, 108, 116, 84, 111, 80, 114, 101, 118, 101, 110, 116, 69, 70, 69, 114, 114, 111, 114, 115 },
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.HasOne("CardTrackerWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
