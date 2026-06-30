using GymApp.Domain.Users;

namespace GymApp.App.Users;

public class LoginUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        ValidateRequest(request);

        var email = new Email(request.Email);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
        return new LoginUserResponse(user.Id, user.Email.ToString(), "made-uptoken");

    }
    private static void ValidateRequest(LoginUserRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

    }
}