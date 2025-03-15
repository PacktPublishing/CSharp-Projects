using Microsoft.EntityFrameworkCore;

namespace CardTrackerWebApi.Repositories;

public class UserRepository(CardTrackerDbContext context)
{
    public int AddUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();

        return user.Id;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await context.Users.ToListAsync();
    }
}