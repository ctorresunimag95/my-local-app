using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Azure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Cinema.Management.Integration.Tests;

public sealed class CinemaApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;
    
    public IResourceBuilder<AzureServiceBusResource> ServiceBus { get; private set; }

    public CinemaApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(CinemaApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);
        
        _app = appBuilder.Build();
    }
    
    public async Task InitializeAsync()
    {
        await _app.StartAsync();;
    }

    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _app.StopAsync();
        if (_app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _app.Dispose();
        }
    }
}