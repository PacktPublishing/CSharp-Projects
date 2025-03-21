﻿// <auto-generated />
using CardTrackerWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    [DbContext(typeof(CardTrackerDbContext))]
    [Migration("20250321021417_ChangeDeckToUserNavigation")]
    partial class ChangeDeckToUserNavigation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("CardTrackerWebApi.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.CardDeck", b =>
                {
                    b.Property<int>("CardId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeckId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.HasKey("CardId", "DeckId");

                    b.HasIndex("DeckId");

                    b.ToTable("CardDeck");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Decks");

                    b.UseTpcMappingStrategy();
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

            modelBuilder.Entity("CardTrackerWebApi.Models.ActionCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<int>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Effect")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("ActionCards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.CreatureCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<bool>("CanClimb")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanFly")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanSwim")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PerTurnEffect")
                        .HasColumnType("TEXT");

                    b.Property<int>("Power")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SummonCost")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SummonEffect")
                        .HasColumnType("TEXT");

                    b.ToTable("CreatureCards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.CardDeck", b =>
                {
                    b.HasOne("CardTrackerWebApi.Models.Card", null)
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CardTrackerWebApi.Models.Deck", null)
                        .WithMany("CardDecks")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.HasOne("CardTrackerWebApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.Navigation("CardDecks");
                });
#pragma warning restore 612, 618
        }
    }
}
