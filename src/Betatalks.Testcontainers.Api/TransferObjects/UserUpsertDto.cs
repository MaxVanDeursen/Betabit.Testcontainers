namespace Betatalks.Testcontainers.Api.TransferObjects;

public record UserUpsertDto(string Name, string UserName, string Email, DateOnly DateOfBirth) { }
