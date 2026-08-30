using GymApp.App.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GymApp.Api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly DeleteUserService _deleteUserService;

    public UsersController(DeleteUserService deleteUserService)
    {
        _deleteUserService = deleteUserService;
    }

    private (string? UserId, string? Email) ExtractUserClaims()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        return (userId, email);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser()
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        try
        {
            await _deleteUserService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NoContent();
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var (userIdClaim, userEmailClaim) = ExtractUserClaims();

        if (!Guid.TryParse(userIdClaim, out var userId) || string.IsNullOrWhiteSpace(userEmailClaim))
        {
            return Unauthorized();
        }

        return Ok(new { Id = userId, Email = userEmailClaim });
    }
}
