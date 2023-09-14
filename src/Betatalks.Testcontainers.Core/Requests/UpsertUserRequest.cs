namespace Betatalks.Testcontainers.Core.Requests;

public record UpsertUserRequest(string Name, string UserName, string Email, DateOnly DateOfBirth) { }
