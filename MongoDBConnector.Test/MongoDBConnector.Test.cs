using System;
using System.Threading.Tasks;
using Testcontainers.MongoDb;
using MongoDBConnector;
using Xunit;

public class MongoConnectionHelperTests : IAsyncLifetime
{
    private readonly MongoDbContainer _container;

    public MongoConnectionHelperTests()
    {
        _container = new MongoDbBuilder()
            .WithImage("mongo:7")
            .WithCleanUp(true)   // remove container after tests
            .Build();
    }

    /// <summary>
    /// Start MongoDB container before running tests.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    /// <summary>
    /// Stop and clean up MongoDB container after tests.
    /// </summary>
    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    [Fact(DisplayName = "CheckConnectionAsync should return true when MongoDB is available")]
    public async Task CheckConnectionAsync_ShouldReturnTrue_WhenMongoIsRunning()
    {
        var service = new MongoConnectionHelper(_container.GetConnectionString());

        var isConnected = await service.CheckConnectionAsync();

        Assert.True(isConnected);
    }

    [Fact(DisplayName = "CheckConnectionAsync should return false for invalid connection string")]
    public async Task CheckConnectionAsync_ShouldReturnFalse_WhenUsingBadConnection()
    {
        const string invalidUri = "mongodb://localhost:12345";
        var service = new MongoConnectionHelper(invalidUri);

        var isConnected = await service.CheckConnectionAsync();

        Assert.False(isConnected);
    }
}
