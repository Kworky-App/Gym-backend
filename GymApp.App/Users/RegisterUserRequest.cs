using GymApp.Domain.Users;

namespace GymApp.App.Users;

public record RegisterUserRequest(
    string Name,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    string Password)
{
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(Password))
            throw new ArgumentException("Password is required.");
    }
}