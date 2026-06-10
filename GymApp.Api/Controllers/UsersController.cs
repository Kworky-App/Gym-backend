using GymApp.App.Users;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Api.Controllers;
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly DeleteUserService _deleteUserService;
    
    public UsersController(DeleteUserService deleteUserService)
    {
        _deleteUserService = deleteUserService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _deleteUserService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}