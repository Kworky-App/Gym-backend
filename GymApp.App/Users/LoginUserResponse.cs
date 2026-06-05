namespace GymApp.App.Users;

public record LoginUserResponse(
    Guid Id,
    string Email,
    string Token);