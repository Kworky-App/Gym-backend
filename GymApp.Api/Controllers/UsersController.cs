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
        catch(InvalidOperationException)
        {
            return NoContent();
        }
    }
}