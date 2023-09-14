using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Betatalks.Testcontainers.Core.Requests;

namespace Betatalks.Testcontainers.Core.Entities;

public class User
{
    private const int nameMaxLength = 150;
    private const int userNameMaxLength = 50;
    private const int emailMaxLength = 320;


    public User(string name, string userName, MailAddress email, DateOnly dateOfBirth)
        : this(Guid.NewGuid(), name, userName, email, dateOfBirth)
    {
    }

    public User(Guid id, string name, string userName, MailAddress email, DateOnly dateOfBirth)
    {
        Id = id;
        Name = ThrowIfNullOrInvalidLength(name, 1, nameMaxLength, nameof(name));
        UserName = ThrowIfNullOrInvalidLength(userName, 1, userNameMaxLength, nameof(userName));
        Email = email == null ? throw new ArgumentNullException(nameof(email)) : email.ToString();

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("The date of birth cannot be in the future", nameof(dateOfBirth));
        }
        DateOfBirth = dateOfBirth;
    }

#pragma warning disable CS8618
    private User() { } // Required by Entity Framework Core.
#pragma warning restore CS8618

    public Guid Id { get; private set; }

    [MaxLength(nameMaxLength)]
    public string Name { get; private set; }

    [MaxLength(userNameMaxLength)]
    public string UserName { get; private set; }

    [MaxLength(emailMaxLength)]
    public string Email { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    private static string ThrowIfNullOrInvalidLength(string value, int minLength, int maxLength, string paramName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(paramName);
        }

        var length = value.Length;
        if (length < minLength || length > maxLength)
        {
            throw new ArgumentException($"The {paramName} should be between {minLength} and {maxLength} characters, but was {length} characters", paramName);
        }

        return value;
    }
}
