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
    .WithEnvironment("ServiceBusConnectionString", serviceBus.Resource.ConnectionStringExpression)
    .WithEnvironment("CosmosDb__Endpoint",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
    .WithEnvironment("CosmosDb__Endpoint", "https://localhost:49254/")
    .WithEnvironment("CosmosEndpointTest", cosmos.Resource.GetEndpoint("emulator"))
    .WaitFor(serviceBus)
    .WaitFor(cosmos);

var reservation = builder.AddProject<Projects.Cinema_Reservation>("reservation")
    .WithEnvironment("RedisConnectionString", cache.Resource.ConnectionStringExpression)
    .WithEnvironment("ServiceBusConnectionString", serviceBus.Resource.ConnectionStringExpression)
    .WaitFor(cache)
    .WaitFor(serviceBus)
    // Added this wait to avoid service bus connectivity error. Waiting for management will help to wait SB being ready
    .WaitFor(management);

builder.Build().Run();