using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class Party : IMapEntity, ICombatGroup
{
    public Pos Position { get; set; } = new(0, 0);
    public List<GameCharacter> Members { get; } = // TODO: This should come from JSON
    [
        new PlayerCharacter
        {
            Name = "Sam the Terramancer",
            Health = 10,
            MaxHealth = 10,
            Mana = 12,
            MaxMana = 12,
            Dexterity = 10,
            Speed = 8,
            Strength = 9,
            Intelligence = 15,
            AsciiArt = [
                @"@ O  ",
                @"|-|-*",
                @"|/ \ "
            ]
        },

        new PlayerCharacter
        {
            Name = "Stephanie the Paladin",
            Health = 20,
            MaxHealth = 20,
            Mana = 5,
            MaxMana = 5,
            Dexterity = 8,
            Speed = 7,
            Strength = 13,
            Intelligence = 10,
            AsciiArt = [
                @"  O~ ",
                @"\-|#  ",
                @" / \ "
            ]
        },

        new PlayerCharacter
        {
            Name = "James the Goat Herder",
            Health = 10,
            MaxHealth = 10,
            Mana = 6,
            MaxMana = 6,
            Dexterity = 8,
            Speed = 8,
            Strength = 8,
            Intelligence = 8,
            AsciiArt = [
                @"^ O       ",
                @"|-|-      ",
                @"| /\ g~ g~"
            ]
        },

        new PlayerCharacter
        {
            Name = "Sara of the Knives",
            Health = 12,
            MaxHealth = 12,
            Mana = 3,
            MaxMana = 3,
            Dexterity = 12,
            Speed = 12,
            Strength = 8,
            Intelligence = 10,
            AsciiArt = [
                @"  O~  ",
                @"\-|-/ ",
                @" / \  "
            ]
        }
    ];

    public string Name => "The Party";

    public bool IsDead => Members.All(m => m.IsDead);
}