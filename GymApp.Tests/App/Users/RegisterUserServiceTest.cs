using GymApp.App.Users;
using GymApp.Domain.Users;
using GymApp.Tests.Fakes;
using GymApp.Tests.Fakes.Users;
using Xunit;

namespace GymApp.Tests.App.Users;

public class RegisterUserServiceTests
{
    private const string ValidName = "John Doe";
    private const string ValidEmail = "john.doe@test.com";
    private const string ValidPassword = "Password123!";
    private const string PasswordHash = "hashed-password";

    private static readonly DateOnly ValidDateOfBirth =
        new(2000, 1, 1);

    private const Gender ValidGender = Gender.Male;

    private readonly FakeUserRepository _userRepository = new();
    private readonly FakePasswordHasher _passwordHasher = new();

    private RegisterUserService CreateService()
    {
        return new RegisterUserService(
            _userRepository,
            _passwordHasher);
    }

    private static RegisterUserRequest CreateValidRequest()
    {
        return new RegisterUserRequest(
            ValidName,
            ValidDateOfBirth,
            ValidGender,
            ValidEmail,
            ValidPassword);
    }

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ShouldRegisterUser()
    {

        var service = CreateService();
        var request = CreateValidRequest();


        var response = await service.RegisterAsync(request);


        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(ValidName, response.Name);
        Assert.Equal(ValidEmail, response.Email);
        Assert.Equal(ValidGender.ToString(), response.Gender);
    }

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ShouldAddUserToRepository()
    {

        var service = CreateService();
        var request = CreateValidRequest();


        await service.RegisterAsync(request);

        var user = await _userRepository.GetByEmailAsync(
            new Email(ValidEmail));

        Assert.NotNull(user);
    }

    [Fact]
    public async Task RegisterAsync_ShouldStoreHashedPassword()
    {

        var service = CreateService();
        var request = CreateValidRequest();

        await service.RegisterAsync(request);

        var user = await _userRepository.GetByEmailAsync(
            new Email(ValidEmail));

        Assert.NotNull(user);
        Assert.Equal(_passwordHasher.Hash(ValidPassword), user.PasswordHash);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnCreatedAt()
    {
        var service = CreateService();
        var request = CreateValidRequest();


        var response = await service.RegisterAsync(request);

        Assert.False(string.IsNullOrWhiteSpace(response.CreatedAt));
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
        var service = CreateService();
        var request = CreateValidRequest();

        await service.RegisterAsync(request);

        var act = () => service.RegisterAsync(request);

        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }

    [Fact]
    public async Task RegisterAsync_WithBlankName_ShouldThrowArgumentException()
    {
        const string blankName = "";

        var service = CreateService();

        var request = new RegisterUserRequest(
            blankName,
            ValidDateOfBirth,
            ValidGender,
            ValidEmail,
            ValidPassword);

        var act = () => service.RegisterAsync(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task RegisterAsync_WithBlankEmail_ShouldThrowArgumentException()
    {
        const string blankEmail = "";

        var service = CreateService();

        var request = new RegisterUserRequest(
            ValidName,
            ValidDateOfBirth,
            ValidGender,
            blankEmail,
            ValidPassword);

        var act = () => service.RegisterAsync(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task RegisterAsync_WithBlankPassword_ShouldThrowArgumentException()
    {
        const string blankPassword = "";

        var service = CreateService();

        var request = new RegisterUserRequest(
            ValidName,
            ValidDateOfBirth,
            ValidGender,
            ValidEmail,
            blankPassword);

        var act = () => service.RegisterAsync(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task RegisterAsync_WithDefaultDateOfBirth_ShouldThrowArgumentException()
    {
        var service = CreateService();

        var request = new RegisterUserRequest(
            ValidName,
            default,
            ValidGender,
            ValidEmail,
            ValidPassword);

        var act = () => service.RegisterAsync(request);

        await Assert.ThrowsAsync<ArgumentException>(act);
    }

}