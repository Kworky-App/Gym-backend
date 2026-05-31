namespace DefaultNamespace;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Email Email { get; private set; }
    public Gender Gender { get; private set; }
    public int Age => CalculateAge();
    public string PasswordHash { get; private set; }
    
    private User() {}

    public User(
        string name,
        DateOnly dateOfBirth,
        Email email,
        Gender gender,
        string passwordHash, )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidUserException("Name cannot be blank.");

        if (email is null)
            throw new InvalidUserException("Email cannot be null.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new InvalidUserException("Password hash cannot be blank.");
        
        Id = Guid.NewGuid();
        Name = name;
        DateOfBirth = dateOfBirth;
        Email = email;
        Gender = gender;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow();
    }
    private int CalculateAge()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - DateOfBirth.Year;

        if (DateOfBirth > today.AddYears(-age))
            age--;

        return age;
    }
}