using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UsingTPCStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionCards_Cards_Id",
                table: "ActionCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardDeck_Cards_CardId",
                table: "CardDeck");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureCards_Cards_Id",
                table: "CreatureCards");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CreatureCards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "CreatureCards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CreatureCards",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ActionCards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ActionCards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ActionCards",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CreatureCards");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "CreatureCards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CreatureCards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ActionCards");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ActionCards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ActionCards");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ActionCards_Cards_Id",
                table: "ActionCards",
                column: "Id",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardDeck_Cards_CardId",
                table: "CardDeck",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureCards_Cards_Id",
                table: "CreatureCards",
                column: "Id",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
