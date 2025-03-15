using System.ComponentModel.DataAnnotations;

namespace CardTrackerWebApi.Models;

public record User
{
    [Key]
    public int Id { get; set; }
    public required string Username { get; set; }
}