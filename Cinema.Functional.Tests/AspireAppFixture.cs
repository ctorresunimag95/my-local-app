using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Cinema.Functional.Tests;

[CollectionDefinition("AspireAppCollection", DisableParallelization = true)]
public class AspireAppCollection : ICollectionFixture<AspireAppFixture>;

public class AspireAppFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(240);
    
    private IDistributedApplicationTestingBuilder _appHost = null!;
    private DistributedApplication _app = null!;

    public HttpClient GatewayClient { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Cinema_AppHost>(cancellationToken);
        _appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(_appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        _appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    
        _app = await _appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        
        GatewayClient = _app.CreateHttpClient("gateway");
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("gateway", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
    }

    public async Task DisposeAsync()
    {
        await _app.DisposeAsync();
    }
}