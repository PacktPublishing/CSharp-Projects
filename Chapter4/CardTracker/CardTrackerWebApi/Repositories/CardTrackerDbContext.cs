using System.Text;
using CardTrackerWebApi.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CardTrackerWebApi.Repositories
{
    public class CardTrackerDbContext(DbContextOptions<CardTrackerDbContext> options, IHashingService hasher, IOptionsSnapshot<AuthSettings> authSettings) : DbContext(options)
    {
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey("UserId");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            // The system requires an admin user to be present for most things, so seed a starter one here
            byte[] salt = "AdminUserUsesAHardcodedSaltToPreventEFErrors"u8.ToArray();
            string password = authSettings.Value.DefaultAdminPassword;
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Salt = salt,
                PasswordHash = hasher.ComputeHash(password, salt),
                IsAdmin = true
            });
        }
    }
}