using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.SqlEdge;

namespace Betatalks.Testcontainers.IntegrationTests;
public class TestcontainersApiContainer : IAsyncDisposable
{
    private const string imageName = "betatalks-testcontainers";

    private const string databaseContainerName = "databaseContainer";

    private const string connectionString = $"Server={databaseContainerName};" +
        $"User ID={SqlEdgeBuilder.DefaultUsername};" +
        $"Password={SqlEdgeBuilder.DefaultPassword};" +
        $"Database={SqlEdgeBuilder.DefaultDatabase};" +
        "TrustServerCertificate=true";

    private INetwork _network;

    private SqlEdgeContainer _databaseContainer;

    private IContainer _apiContainer;

    private TestcontainersApiContainer(INetwork network, SqlEdgeContainer databaseContainer, IContainer apiContainer)
    {
        _network = network;
        _databaseContainer = databaseContainer;
        _apiContainer = apiContainer;
    }

    public static async Task<TestcontainersApiContainer> CreateAsync(CancellationToken cancellationToken = default)
    {
        var image = new ImageFromDockerfileBuilder()
            .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithName(imageName)
            .Build();
        await image.CreateAsync(cancellationToken).ConfigureAwait(false);

        var network = new NetworkBuilder().Build();

        var databaseContainer = new SqlEdgeBuilder()
            .WithNetwork(network)
            .WithNetworkAliases(databaseContainerName)
            .Build();

        var apiContainer = new ContainerBuilder()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
            .WithImage(imageName)
            .WithNetwork(network)
            .WithPortBinding(80, true)
            .WithEnvironment("ConnectionStrings__Database", connectionString)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
            .Build();

        await databaseContainer.StartAsync(cancellationToken).ConfigureAwait(false);
        await apiContainer.StartAsync(cancellationToken).ConfigureAwait(false);

        return new TestcontainersApiContainer(network, databaseContainer, apiContainer);
    }

    public Uri ConstructUri(string subPath) => new UriBuilder(Uri.UriSchemeHttp, _apiContainer.Hostname, _apiContainer.GetMappedPublicPort(80), subPath).Uri;

    public async ValueTask DisposeAsync()
    {
        await _apiContainer.DisposeAsync().ConfigureAwait(false);
        await _databaseContainer.DisposeAsync().ConfigureAwait(false);
        await _network.DisposeAsync().ConfigureAwait(false);
    }
}
