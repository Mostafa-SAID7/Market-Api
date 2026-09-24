namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating User entities for testing.
/// </summary>
public class UserBuilder
{
    private int _id = 1;
    private string _email = "user@example.com";
    private string _firstName = "John";
    private string _lastName = "Doe";
    private string _phoneNumber = "+1234567890";
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a UserBuilder with default values.
    /// </summary>
    public static UserBuilder Default() => new();

    /// <summary>
    /// Creates a UserBuilder with a specific ID.
    /// </summary>
    public static UserBuilder WithId(int id) => new() { _id = id };

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public UserBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the User entity.
    /// </summary>
    public User Build()
    {
        return new User
        {
            Id = _id,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            PhoneNumber = _phoneNumber,
            CreatedAt = _createdAt
        };
    }
}
