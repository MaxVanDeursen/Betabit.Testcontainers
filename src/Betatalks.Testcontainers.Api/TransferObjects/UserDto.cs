namespace Betatalks.Testcontainers.Api.TransferObjects;

public record UserDto(Guid Id, string Name, string UserName, string Email, DateOnly DateOfBirth)
{

}
