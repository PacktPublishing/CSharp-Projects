using System.ComponentModel.DataAnnotations;

namespace CardTrackerWebApi.Models;

public abstract record Card
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}