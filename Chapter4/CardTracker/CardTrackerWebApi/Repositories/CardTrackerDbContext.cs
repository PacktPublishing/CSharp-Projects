using Microsoft.EntityFrameworkCore;

namespace CardTrackerWebApi.Repositories
{
    public class CardTrackerDbContext(DbContextOptions<CardTrackerDbContext> options) : DbContext(options)
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
        }
    }
}