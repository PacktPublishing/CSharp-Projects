using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class MultipleCopiesOfCardsPerDeck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck");

            migrationBuilder.RenameColumn(
                name: "CardsId",
                table: "CardDeck",
                newName: "Count");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "CardDeck",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck",
                columns: new[] { "CardId", "DeckId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "CardDeck");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "CardDeck",
                newName: "CardsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck",
                columns: new[] { "CardsId", "DeckId" });
        }
    }
}
