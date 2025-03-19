using System.ComponentModel.DataAnnotations;

namespace CardTrackerWebApi.Models;

[Index(nameof(Username), IsUnique = true)]
public record User
{
    [Key]
    public int Id { get; set; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] Salt { get; set; }
    public bool IsAdmin { get; set; }
}