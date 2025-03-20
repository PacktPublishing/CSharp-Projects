using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyExamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeCards");

            migrationBuilder.DropTable(
                name: "EventCards");

            migrationBuilder.DropTable(
                name: "FriendCards");

            migrationBuilder.DropTable(
                name: "LocationCards");

            migrationBuilder.DropColumn(
                name: "DeckType",
                table: "Decks");

            migrationBuilder.CreateTable(
                name: "CreatureCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SummonEffect = table.Column<string>(type: "TEXT", nullable: true),
                    PerTurnEffect = table.Column<string>(type: "TEXT", nullable: true),
                    SummonCost = table.Column<int>(type: "INTEGER", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    CanFly = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSwim = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanClimb = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureCards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreatureCards");

            migrationBuilder.AddColumn<int>(
                name: "DeckType",
                table: "Decks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChallengeCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FoodRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    KnowledgeRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    PerTurnEffect = table.Column<string>(type: "TEXT", nullable: true),
                    ResolvedEffect = table.Column<string>(type: "TEXT", nullable: true),
                    RevealedEffect = table.Column<string>(type: "TEXT", nullable: true),
                    VictoryPointsOnCompleted = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Effect = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CanClimb = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanFly = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSwim = table.Column<bool>(type: "INTEGER", nullable: false),
                    FoodPerTurn = table.Column<int>(type: "INTEGER", nullable: false),
                    PerTurnEffect = table.Column<string>(type: "TEXT", nullable: true),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    SummonEffect = table.Column<string>(type: "TEXT", nullable: true),
                    SummonFoodCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SummonWisdomCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Challenges = table.Column<int>(type: "INTEGER", nullable: false),
                    ExploredEffect = table.Column<string>(type: "TEXT", nullable: true),
                    RevealedEffect = table.Column<string>(type: "TEXT", nullable: true),
                    VictoryPointsOnExplored = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCards", x => x.Id);
                });
        }
    }
}
