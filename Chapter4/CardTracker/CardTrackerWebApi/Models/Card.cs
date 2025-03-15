using System.ComponentModel.DataAnnotations;

namespace CardTrackerWebApi.Models;

public record Card
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
}