﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    [DbContext(typeof(CardTrackerDbContext))]
    [Migration("20250318033129_InheritanceHierarchy")]
    partial class InheritanceHierarchy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("CardDeck", b =>
                {
                    b.Property<int>("CardsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeckId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CardsId", "DeckId");

                    b.HasIndex("DeckId");

                    b.ToTable("CardDeck");
                });

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

            modelBuilder.Entity("CardTrackerWebApi.Models.Deck", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeckType")
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

            modelBuilder.Entity("CardTrackerWebApi.Models.ChallengeCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<int>("FoodRequired")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KnowledgeRequired")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PerTurnEffect")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResolvedEffect")
                        .HasColumnType("TEXT");

                    b.Property<string>("RevealedEffect")
                        .HasColumnType("TEXT");

                    b.Property<int>("VictoryPointsOnCompleted")
                        .HasColumnType("INTEGER");

                    b.ToTable("ChallengeCards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.EventCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<string>("Effect")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("EventCards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.FriendCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<bool>("CanClimb")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanFly")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanSwim")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodPerTurn")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PerTurnEffect")
                        .HasColumnType("TEXT");

                    b.Property<int>("Power")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SummonEffect")
                        .HasColumnType("TEXT");

                    b.Property<int>("SummonFoodCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SummonWisdomCost")
                        .HasColumnType("INTEGER");

                    b.ToTable("FriendCards");
                });

            modelBuilder.Entity("CardTrackerWebApi.Models.LocationCard", b =>
                {
                    b.HasBaseType("CardTrackerWebApi.Models.Card");

                    b.Property<int>("Challenges")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExploredEffect")
                        .HasColumnType("TEXT");

                    b.Property<string>("RevealedEffect")
                        .HasColumnType("TEXT");

                    b.Property<int>("VictoryPointsOnExplored")
                        .HasColumnType("INTEGER");

                    b.ToTable("LocationCards");
                });

            modelBuilder.Entity("CardDeck", b =>
                {
                    b.HasOne("CardTrackerWebApi.Models.Card", null)
                        .WithMany()
                        .HasForeignKey("CardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CardTrackerWebApi.Models.Deck", null)
                        .WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
