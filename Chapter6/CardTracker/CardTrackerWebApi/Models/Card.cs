using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTrackerWebApi.Models;

public abstract class Card
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public List<Deck> Decks { get; set; } = new();
    public string CardType => GetType().Name;
}