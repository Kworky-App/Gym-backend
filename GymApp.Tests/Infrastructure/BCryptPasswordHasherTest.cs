using GymApp.Infrastructure.Users;
using Xunit;

namespace GymApp.Tests.Infrastructure;

public class BCryptPasswordHasherTests
{
    private const string ValidPassword = "Password123!";
    private const string InvalidPassword = "WrongPassword123!";

    [Fact]
    public void Hash_WithValidPassword_ShouldReturnPasswordHash()
    {
        
        var passwordHasher = new BCryptPasswordHasher();
        
        var passwordHash = passwordHasher.Hash(ValidPassword);
        
        Assert.False(string.IsNullOrWhiteSpace(passwordHash));
        Assert.NotEqual(ValidPassword, passwordHash);
    }

    [Fact]
    public void Verify_WithValidPasswordAndMatchingHash_ShouldReturnTrue()
    {
        
        var passwordHasher = new BCryptPasswordHasher();
        var passwordHash = passwordHasher.Hash(ValidPassword);
        
        var result = passwordHasher.Verify(ValidPassword, passwordHash);
        
        Assert.True(result);
    }

    [Fact]
    public void Verify_WithInvalidPasswordAndHash_ShouldReturnFalse()
    {
        
        var passwordHasher = new BCryptPasswordHasher();
        var passwordHash = passwordHasher.Hash(ValidPassword);
        
        var result = passwordHasher.Verify(InvalidPassword, passwordHash);
        
        Assert.False(result);
    }

    [Fact]
    public void Hash_WithSamePasswordTwice_ShouldReturnDifferentHashes()
    {
        var passwordHasher = new BCryptPasswordHasher();
        
        var firstPasswordHash = passwordHasher.Hash(ValidPassword);
        var secondPasswordHash = passwordHasher.Hash(ValidPassword);
        
        Assert.NotEqual(firstPasswordHash, secondPasswordHash);
    }
}