using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Betatalks.Testcontainers.IntegrationTests;

public class UserApiTests
{
    private const string userApiPath = "api/users";

    [Fact]
    public async Task Should_return_empty_list_of_users()
    {
        await using var container = await TestcontainersApiContainer.CreateAsync();
        using var httpClient = new HttpClient();
        var url = container.ConstructUri(userApiPath);

        var response = await httpClient
          .GetAsync(url)
          .ConfigureAwait(false);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var deserializedUsers = JsonSerializer.Deserialize<List<object>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        deserializedUsers.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_not_found_for_unknown_user()
    {
        await using var container = await TestcontainersApiContainer.CreateAsync();
        using var httpClient = new HttpClient();
        var url = container.ConstructUri($"{userApiPath}/{Guid.NewGuid()}");

        var response = await httpClient
          .GetAsync(url)
          .ConfigureAwait(false);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_create_and_return_user()
    {
        await using var container = await TestcontainersApiContainer.CreateAsync();
        using var httpClient = new HttpClient();
        var url = container.ConstructUri(userApiPath);
        var userBody = JsonSerializer.Serialize(new
        {
            Name = "name",
            UserName = "userName",
            Email = "email@email.com",
            DateOfBirth = "2000-01-01"
        });
        var buffer = System.Text.Encoding.UTF8.GetBytes(userBody);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var createResponse = await httpClient
          .PostAsync(url, byteContent)
          .ConfigureAwait(false);
        var retrieveUrl = createResponse.Headers
            .SingleOrDefault(header => header.Key == "Location")
            .Value?.SingleOrDefault();
        var retrieveResponse = await httpClient
            .GetAsync(retrieveUrl)
            .ConfigureAwait(false);

        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        retrieveResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var serializedUser = await retrieveResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var deserializedUser = JsonSerializer.Deserialize<User>(serializedUser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        deserializedUser.Should().NotBeNull().And.BeEquivalentTo(new User("name", "userName", "email@email.com", "2000-01-01"));
    }
}
