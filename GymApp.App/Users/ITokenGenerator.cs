using GymApp.Domain.Users;

namespace GymApp.App.Users;

public interface ITokenGenerator
{
    string Generate(User user);
}