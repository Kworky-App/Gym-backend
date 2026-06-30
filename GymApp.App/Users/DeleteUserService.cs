using GymApp.Domain.Users;

namespace GymApp.App.Users;

public class DeleteUserService
{
    private readonly IUserRepository _userRepository;


    public DeleteUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }
        await _userRepository.DeleteAsync(user);
    }

}