using System.Security.Claims;
using GymApp.Api.Controllers;
using GymApp.App.Users;
using GymApp.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Xunit;

namespace GymApp.Tests.Api.Users;

public class UsersControllerTests
{
    private static readonly Guid ValidUserId = Guid.NewGuid();
    private const string ValidEmail = "john.doe@test.com";

    private static UsersController CreateController(ClaimsPrincipal user)
    {
        var deleteUserService = new DeleteUserService(new FakeUserRepository());

        return new UsersController(deleteUserService)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            }
        };
    }

    private static ClaimsPrincipal CreatePrincipal(string? userId, string? email)
    {
        var claims = new List<Claim>();

        if (userId is not null)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
        }

        if (email is not null)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }

    [Fact]
    public async Task GetCurrentUser_WithValidClaims_ShouldReturnOkWithIdAndEmail()
    {
        var controller = CreateController(CreatePrincipal(ValidUserId.ToString(), ValidEmail));

        var result = await controller.GetCurrentUser();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value!;
        var idProperty = value.GetType().GetProperty("Id");
        var emailProperty = value.GetType().GetProperty("Email");

        Assert.Equal(ValidUserId, idProperty?.GetValue(value));
        Assert.Equal(ValidEmail, emailProperty?.GetValue(value));
    }

    [Fact]
    public async Task GetCurrentUser_WithMissingUserIdClaim_ShouldReturnUnauthorized()
    {
        var controller = CreateController(CreatePrincipal(null, ValidEmail));

        var result = await controller.GetCurrentUser();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetCurrentUser_WithInvalidUserIdClaim_ShouldReturnUnauthorized()
    {
        var controller = CreateController(CreatePrincipal("not-a-guid", ValidEmail));

        var result = await controller.GetCurrentUser();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetCurrentUser_WithMissingEmailClaim_ShouldReturnUnauthorized()
    {
        var controller = CreateController(CreatePrincipal(ValidUserId.ToString(), null));

        var result = await controller.GetCurrentUser();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetCurrentUser_WithBlankEmailClaim_ShouldReturnUnauthorized()
    {
        var controller = CreateController(CreatePrincipal(ValidUserId.ToString(), " "));

        var result = await controller.GetCurrentUser();

        Assert.IsType<UnauthorizedResult>(result);
    }
}
