namespace GymApp.App.Users;

public record LoginUserRequest(
    string Email,
    string Password);