using System.Text;
using CardTrackerWebApi.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CardTrackerWebApi.Repositories
{
    public class CardTrackerDbContext(DbContextOptions<CardTrackerDbContext> options, IHashingService hasher, IOptionsSnapshot<AuthSettings> authSettings) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        
        public DbSet<Card> Cards { get; set; }
        public DbSet<ActionCard> ActionCards { get; set; }
        public DbSet<ChallengeCard> ChallengeCards { get; set; }
        public DbSet<FriendCard> FriendCards { get; set; }
        public DbSet<LocationCard> LocationCards { get; set; }
        public DbSet<EventCard> EventCards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define one to many relationship between User and Deck
            modelBuilder.Entity<Deck>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey("UserId");
            
            // Define many-to-many relationships where each deck has multiple cards and each card can belong to multiple decks
            // See https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
            modelBuilder.Entity<Deck>()
                .HasMany(d => d.Cards)
                .WithMany(); // Unidirectional - cards don't need to navigate back to decks they're in
            
            // Use Table per Concrete Type mapping strategy for inheritance
            modelBuilder.Entity<Deck>().UseTpcMappingStrategy();
            modelBuilder.Entity<Card>().UseTpcMappingStrategy();
            
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