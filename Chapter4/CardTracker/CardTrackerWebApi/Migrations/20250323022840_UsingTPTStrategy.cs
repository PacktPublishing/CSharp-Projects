using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UsingTPTStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanClimb",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CanFly",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CanSwim",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Effect",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PerTurnEffect",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SummonCost",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SummonEffect",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "ActionCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Effect = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionCards_Cards_Id",
                        column: x => x.Id,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    table.ForeignKey(
                        name: "FK_CreatureCards_Cards_Id",
                        column: x => x.Id,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionCards");

            migrationBuilder.DropTable(
                name: "CreatureCards");

            migrationBuilder.AddColumn<bool>(
                name: "CanClimb",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanFly",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanSwim",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Cards",
                type: "TEXT",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Effect",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PerTurnEffect",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SummonCost",
                table: "Cards",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SummonEffect",
                table: "Cards",
                type: "TEXT",
                nullable: true);
        }
    }
}
