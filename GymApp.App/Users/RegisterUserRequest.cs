using GymApp.Domain.Users;

namespace GymApp.App.Users;

public record RegisterUserRequest(
    string Name,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    string Password
);