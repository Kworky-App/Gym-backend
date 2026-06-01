using GymApp.Domain.Users.Exceptions;

namespace GymApp.Domain.Users;

public sealed class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException("Email is required");

        if (!value.Contains("@"))
            throw new InvalidEmailException("Email is invalid");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}