using GymApp.App.Users;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserService _registerUserService;

    public AuthController(RegisterUserService registerUserService)
    {
        _registerUserService = registerUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        try
        {
            var user = await _registerUserService.RegisterAsync(request);
            return Created("/auth/register", new
            {
                id = user.Id,
                name = user.Name,
                dateOfBirth = user.DateOfBirth,
                gender = user.Gender.ToString(),
                email = user.Email.Value,
                createdAt = user.CreatedAt
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new{message = exception.Message});
        }
        catch(InvalidOperationException exception)
        {
            return Conflict(new{message = exception.Message});
        }
    }
}