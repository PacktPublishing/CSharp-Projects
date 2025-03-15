using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CardTrackerWebApi.Models;

public record User
{
    [Key]
    public int Id { get; set; }
    
    public required string Username { get; set; }
}