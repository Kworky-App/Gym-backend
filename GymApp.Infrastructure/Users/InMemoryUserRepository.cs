using GymApp.Domain.Users;

namespace GymApp.Infrastructure.Users;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByEmailAsync(Email email)
    {
        var user = _users.FirstOrDefault(user =>user.Email.Value == email.Value);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}