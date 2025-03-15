using Microsoft.EntityFrameworkCore;

namespace CardTrackerWebApi.Repositories
{
    public class CardTrackerDbContext(DbContextOptions<CardTrackerDbContext> options) : DbContext(options)
    {
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
    }
}