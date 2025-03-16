using Microsoft.EntityFrameworkCore;

namespace CardTrackerWebApi.Repositories;

public class UserRepository(CardTrackerDbContext context)
{
    public User AddUser(User user)
    {
        if (context.Users.Any(u => u.Username == user.Username)) {
            throw new InvalidOperationException("User already exists with this username");
        }
        
        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserAsync(string username)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username.ToLowerInvariant());
    }
}