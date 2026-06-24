using GymApp.Domain.Users;

namespace GymApp.Tests.Fakes.Users;

public class FakePasswordHasher : IPasswordHasher
{
    public const string PasswordHash = "hashed-password";

    public string Hash(string password)
    {
        return PasswordHash;
    }

    public bool Verify(string password, string passwordHash)
    {
        return passwordHash == PasswordHash;
    }
}