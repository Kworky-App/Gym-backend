namespace GymApp.App.Users;

public record RegisterUserResponse(
    
    Guid Id,
    string Name,
    string Email,
    string Gender,
    string CreatedAt);