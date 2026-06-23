using GymApp.Domain.Users;

namespace GymApp.App.Users;

public class RegisterUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
    {
        ValidateRequest(request);

        var email = new Email(request.Email);

        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser is not null)
            throw new InvalidOperationException(
                $"A user with email '{email}' already exists.");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.Name,
            request.DateOfBirth,
            email,
            request.Gender,
            passwordHash
        );

        await _userRepository.AddAsync(user);

        return new RegisterUserResponse(
            user.Id,
            user.Name,
            user.Email.ToString(),
            user.Gender.ToString(),
            FormatQuebecDateTime(user.CreatedAt));
    }
    private static string FormatQuebecDateTime(DateTime createdAt)
    {
        var quebecTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("America/Toronto");

        var utcDateTime =
            DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);

        var quebecTime =
            TimeZoneInfo.ConvertTimeFromUtc(
                utcDateTime,
                quebecTimeZone);

        return quebecTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static void ValidateRequest(RegisterUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        if (request.DateOfBirth == default)
            throw new ArgumentException("Date of birth is required.");
    }
}