using GymApp.Domain.Users;

namespace GymApp.Tests.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByEmailAsync(Email email)
    {
        var user = _users.FirstOrDefault(user =>
            user.Email.Value == email.Value);

        return Task.FromResult(user);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(user =>
            user.Id == id);

        return Task.FromResult(user);
    }

    public Task AddAsync(User user)
    {
        _users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        _users.Remove(user);

        return Task.CompletedTask;
    }
}