namespace GymApp.Domain.Users.Exceptions;

public class InvalidUserException:Exception
{
    public InvalidUserException(string message)
        : base(message)
    {
    }
}