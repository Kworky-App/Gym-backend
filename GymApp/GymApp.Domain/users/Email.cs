namespace DefaultNamespace;

public sealed class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.");

        if (!value.Contains("@"))
            throw new ArgumentException("Email is invalid.");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}