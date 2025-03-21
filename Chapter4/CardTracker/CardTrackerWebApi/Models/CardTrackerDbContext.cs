using CardTrackerWebApi.Settings;
using Microsoft.Extensions.Options;

namespace CardTrackerWebApi.Models
{
    public class CardTrackerDbContext(DbContextOptions<CardTrackerDbContext> options, IHashingService hasher, IOptionsSnapshot<AuthSettings> authSettings) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        
        public DbSet<Card> Cards { get; set; }
        public DbSet<ActionCard> ActionCards { get; set; }
        public DbSet<CreatureCard> CreatureCards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define one-to-many relationship between User and Deck
            modelBuilder.Entity<Deck>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId");
            
            // Define many-to-many relationships where each deck has multiple cards and each card can belong to multiple decks
            // See https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
            modelBuilder.Entity<Deck>()
                .HasMany(d => d.Cards)
                .WithMany(c => c.Decks)
                .UsingEntity<CardDeck>();
            
            // Use Table per Hierarchy mapping strategy for inheritance
            // See https://learn.microsoft.com/en-us/ef/core/modeling/inheritance
            modelBuilder.Entity<Deck>().UseTphMappingStrategy();
            modelBuilder.Entity<Card>().UseTphMappingStrategy();
            
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