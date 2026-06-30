using GymApp.App.Users;
using GymApp.Domain.Users;
using GymApp.Tests.Fakes;
using GymApp.Tests.Fakes.Users;
using Xunit;

namespace GymApp.Tests.App.Users;

public class LoginUserServiceTests
{
    private const string ValidEmail = "john.doe@test.com";
    private const string ValidPassword = "Password123!";
    private const string InvalidPassword = "WrongPassword123!";
    private const string PasswordHash = FakePasswordHasher.PasswordHash;
    private const string ExpectedToken = "made-uptoken";

    private const string ValidName = "John Doe";
    private static readonly DateOnly ValidDateOfBirth = new(2000, 1, 1);
    private const Gender ValidGender = Gender.Male;

    private readonly FakeUserRepository _userRepository = new();
    private readonly FakePasswordHasher _passwordHasher = new();

    private LoginUserService CreateService()
    {
        return new LoginUserService(
            _userRepository,
            _passwordHasher);
    }

    private static LoginUserRequest CreateValidRequest()
    {
        return new LoginUserRequest(
            ValidEmail,
            ValidPassword);
    }

    private static User CreateUser()
    {
        var passwordHasher = new FakePasswordHasher();
        return new User(
            ValidName,
            ValidDateOfBirth,
            new Email(ValidEmail),
            ValidGender,
            passwordHasher.Hash(ValidPassword));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnLoginUserResponse()
    {
        var service = CreateService();
        var user = CreateUser();
        var request = CreateValidRequest();

        await _userRepository.AddAsync(user);

        var response = await service.Login(request);

        Assert.Equal(user.Id, response.Id);
        Assert.Equal(ValidEmail, response.Email);
        Assert.Equal(ExpectedToken, response.Token);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ShouldThrowUnauthorizedAccessException()
    {
        var service = CreateService();
        var request = CreateValidRequest();

        var act = () => service.Login(request);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldThrowUnauthorizedAccessException()
    {
        var service = CreateService();
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var request = new LoginUserRequest(
            ValidEmail,
            InvalidPassword);


        var act = () => service.Login(request);


        await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
    }

    [Fact]
    public async Task Login_WithBlankEmail_ShouldThrowArgumentException()
    {

        const string blankEmail = "";

        var service = CreateService();

        var request = new LoginUserRequest(
            blankEmail,
            ValidPassword);

        var act = () => service.Login(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task Login_WithBlankPassword_ShouldThrowArgumentException()
    {
        const string blankPassword = "";

        var service = CreateService();

        var request = new LoginUserRequest(
            ValidEmail,
            blankPassword);

        var act = () => service.Login(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }
}