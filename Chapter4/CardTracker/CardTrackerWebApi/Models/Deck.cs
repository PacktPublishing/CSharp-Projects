using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTrackerWebApi.Models;

public record Deck
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public required User User { get; set; } = null!;
    
    public List<Card> Cards { get; set; } = new();
}