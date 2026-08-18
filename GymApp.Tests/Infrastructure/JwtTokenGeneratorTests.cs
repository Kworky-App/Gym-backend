using System.IdentityModel.Tokens.Jwt;
using GymApp.Domain.Users;
using GymApp.Infrastructure.Authentification;
using Microsoft.Extensions.Options;
using Xunit;

namespace GymApp.Tests.Infrastructure.Authentification;

public class JwtTokenGeneratorTests
{
    private const string Issuer = "GymApp.Api";
    private const string Audience = "GymApp.Mobile";

    private const string SecretKey =
        "GymApp-Test-Secret-Key-That-Is-Definitely-Long-Enough-12345";

    private const int ExpirationMinutes = 15;

    private const string ValidName = "John Doe";
    private const string ValidEmail = "john.doe@test.com";
    private const string PasswordHash = "hashed-password";

    private static readonly DateOnly ValidDateOfBirth =
        new(2000, 1, 1);

    private const Gender ValidGender = Gender.Male;

    private JwtTokenGenerator CreateTokenGenerator()
    {
        var options = Options.Create(
            new JwtOptions
            {
                Issuer = Issuer,
                Audience = Audience,
                SecretKey = SecretKey,
                ExpirationMinutes = ExpirationMinutes
            });

        return new JwtTokenGenerator(options);
    }

    private static User CreateUser()
    {
        return new User(
            ValidName,
            ValidDateOfBirth,
            new Email(ValidEmail),
            ValidGender,
            PasswordHash);
    }

    [Fact]
    public void Generate_WithValidUser_ShouldReturnToken()
    {
        var generator = CreateTokenGenerator();
        var user = CreateUser();

        var token = generator.Generate(user);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void Generate_ShouldContainUserIdAsSubjectClaim()
    {
        var generator = CreateTokenGenerator();
        var user = CreateUser();

        var tokenString = generator.Generate(user);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        var subjectClaim = token.Claims
            .First(claim => claim.Type == "sub");

        Assert.Equal(
            user.Id.ToString(),
            subjectClaim.Value);
    }

    [Fact]
    public void Generate_ShouldContainEmailClaim()
    {
        var generator = CreateTokenGenerator();
        var user = CreateUser();

        var tokenString = generator.Generate(user);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        var emailClaim = token.Claims
            .First(claim => claim.Type == "email");

        Assert.Equal(
            user.Email.Value,
            emailClaim.Value);
    }

    [Fact]
    public void Generate_ShouldUseConfiguredIssuerAndAudience()
    {
        var generator = CreateTokenGenerator();
        var user = CreateUser();

        var tokenString = generator.Generate(user);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        Assert.Equal(
            Issuer,
            token.Issuer);

        Assert.Contains(
            Audience,
            token.Audiences);
    }

    [Fact]
    public void Generate_ShouldSetExpiration()
    {
        var generator = CreateTokenGenerator();
        var user = CreateUser();

        var beforeGeneration = DateTime.UtcNow;

        var tokenString = generator.Generate(user);

        var afterGeneration = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        var expectedMinimum =
            beforeGeneration
                .AddMinutes(ExpirationMinutes)
                .AddSeconds(-1);

        var expectedMaximum =
            afterGeneration
                .AddMinutes(ExpirationMinutes)
                .AddSeconds(1);

        Assert.InRange(
            token.ValidTo,
            expectedMinimum,
            expectedMaximum);
    }
}