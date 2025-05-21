var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight()
    .WithLifetime(ContainerLifetime.Persistent);

var serviceBus = builder.AddAzureServiceBus("serviceBus")
    .RunAsEmulator(emulator =>
    {
        emulator.WithConfigurationFile(
            path: "../containers/service-bus/config.json");

        emulator.WithLifetime(ContainerLifetime.Persistent);
    });

var cosmos = builder.AddAzureCosmosDB("cosmosDb").RunAsEmulator(
    emulator =>
    {
        emulator
            .WithGatewayPort(49254)
            .WithHttpEndpoint(targetPort: 1234, name: "explorer-port")
            .WithDataVolume("cosmos-drive");
        
        emulator.WithLifetime(ContainerLifetime.Persistent);
    });


var management = builder.AddProject<Projects.Cinema_Management>("management")
    .WithReference(serviceBus, connectionName: "serviceBus")
    .WithReference(cosmos, connectionName: "cosmosDb")
    .WaitFor(serviceBus)
    .WaitFor(cosmos);

var reservation = builder.AddProject<Projects.Cinema_Reservation>("reservation")
    .WithReference(cache, "redis")
    .WithReference(serviceBus, "serviceBus")
    .WaitFor(cache)
    .WaitFor(serviceBus)
    // Added this wait to avoid service bus connectivity error. Waiting for management will help to wait SB being ready
    .WaitFor(management);

builder.Build().Run();