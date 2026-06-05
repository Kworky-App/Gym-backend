using GymApp.App.Users;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserService _registerUserService;
    private readonly LoginUserService _loginUserService;

    public AuthController(RegisterUserService registerUserService,LoginUserService loginUserService)
    {
        _registerUserService = registerUserService;
        _loginUserService = loginUserService;
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
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var response = await _loginUserService.Login(request);
            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }

        
    }
}