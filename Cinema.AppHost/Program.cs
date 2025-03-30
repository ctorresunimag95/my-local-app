var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
        .WithRedisInsight()
    // .WithLifetime(ContainerLifetime.Persistent)
    ;

var serviceBus = builder.AddAzureServiceBus("serviceBus")
    .RunAsEmulator(emulator =>
    {
        emulator.WithConfigurationFile(
            path: "../containers/service-bus/config.json");
    });

var cosmos = builder.AddAzureCosmosDB("cosmos-db").RunAsEmulator(
    emulator =>
    {
        emulator
            .WithGatewayPort(49254)
            .WithHttpEndpoint(targetPort: 1234, name: "explorer-port")
            .WithDataVolume("cosmos-drive");
    });


var management = builder.AddProject<Projects.Cinema_Management>("management")
    .WithEnvironment("ConnectionStrings__service-bus", serviceBus.Resource.ConnectionStringExpression)
    .WithEnvironment("ConnectionStrings__cosmos-db",
        cosmos.Resource.ConnectionStringExpression)
    .WaitFor(serviceBus)
    .WaitFor(cosmos);

var reservation = builder.AddProject<Projects.Cinema_Reservation>("reservation")
    .WithEnvironment("ConnectionStrings__redis", cache.Resource.ConnectionStringExpression)
    .WithEnvironment("ConnectionStrings__service-bus", serviceBus.Resource.ConnectionStringExpression)
    .WaitFor(cache)
    .WaitFor(serviceBus)
    // Added this wait to avoid service bus connectivity error. Waiting for management will help to wait SB being ready
    .WaitFor(management);

builder.Build().Run();