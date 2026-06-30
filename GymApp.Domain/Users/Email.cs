using System.Text.RegularExpressions;
using GymApp.Domain.Users.Exceptions;

namespace GymApp.Domain.Users;

public sealed class Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException("Email is required.");

        if (!EmailRegex.IsMatch(value))
            throw new InvalidEmailException("Email is invalid.");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}