using GymApp.Domain.Users;
using GymApp.Infrastructure.Users;
using Xunit;

namespace GymApp.Tests.Infrastructure;

public class InMemoryUserRepositoryTests
{
    private const string ValidName = "John Doe";
    private const string ValidPasswordHash = "password-hash";

    private static readonly DateOnly ValidDateOfBirth =
        new(2000, 1, 1);

    private static readonly Email ValidEmail =
        new("john.doe@test.com");

    private const Gender ValidGender = Gender.Male;

    private readonly InMemoryUserRepository _repository = new();

    private static User CreateUser()
    {
        return new User(
            ValidName,
            ValidDateOfBirth,
            ValidEmail,
            ValidGender,
            ValidPasswordHash);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        var user = CreateUser();
        
        await _repository.AddAsync(user);
        var result = await _repository.GetByIdAsync(user.Id);
        
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUserId_ShouldReturnUser()
    {
        var user = CreateUser();

        await _repository.AddAsync(user);
        
        var result = await _repository.GetByIdAsync(user.Id);
        
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownUserId_ShouldReturnNull()
    {
        var unknownUserId = Guid.NewGuid();
        
        var result = await _repository.GetByIdAsync(unknownUserId);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        var user = CreateUser();

        await _repository.AddAsync(user);
        
        var result = await _repository.GetByEmailAsync(user.Email);
        
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithUnknownEmail_ShouldReturnNull()
    {
        var unknownEmail = new Email("unknown@test.com");
        
        var result = await _repository.GetByEmailAsync(unknownEmail);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        
        var user = CreateUser();

        await _repository.AddAsync(user);

        
        await _repository.DeleteAsync(user);
        var result = await _repository.GetByIdAsync(user.Id);

        
        Assert.Null(result);
    }
}