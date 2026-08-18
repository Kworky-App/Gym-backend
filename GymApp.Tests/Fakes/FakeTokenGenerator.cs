using GymApp.App.Users;
using GymApp.Domain.Users;

namespace GymApp.Tests.Fakes;

public class FakeTokenGenerator : ITokenGenerator
{
    public const string Token = "fake-jwt-token";

    public string Generate(User user)
    {
        return Token;
    }
}