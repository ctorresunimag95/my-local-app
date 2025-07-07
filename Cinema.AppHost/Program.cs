using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithHostPort(55425)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var db = sql.AddDatabase("cinema");

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

var cosmos = builder.AddAzureCosmosDB("cosmosDb").RunAsEmulator(emulator =>
{
    emulator
        .WithGatewayPort(49254)
        .WithHttpEndpoint(targetPort: 1234, name: "explorer-port")
        .WithDataVolume("cosmos-drive");

    emulator.WithLifetime(ContainerLifetime.Persistent);
});

var omdbApiKey = builder.AddParameter("omdbApiKey", builder.Configuration.GetValue<string>("omdb:apiKey")!, secret: true);

var management = builder.AddProject<Projects.Cinema_Management>("management")
    .WithEnvironment("omdb:apiKey", omdbApiKey)
    .WithEnvironment("omdb:apiUrl", "https://www.omdbapi.com/")
    .WithReference(serviceBus, connectionName: "serviceBus")
    .WithReference(cosmos, connectionName: "cosmosDb")
    .WaitFor(serviceBus)
    .WaitFor(cosmos);

var reservationMigrationService = builder.AddProject<Projects.Cinema_Reservation_MigrationWorker>("reservation-migration")
    .WithReference(db, "database")
    .WaitFor(db);

var reservation = builder.AddProject<Projects.Cinema_Reservation>("reservation")
    .WithReference(cache, "redis")
    .WithReference(serviceBus, "serviceBus")
    .WaitFor(cache)
    .WaitFor(serviceBus)
    // Added this wait to avoid service bus connectivity error. Waiting for management will help to wait SB being ready
    .WaitFor(management)
    .WithReference(db, "database")
    .WaitFor(db)
    .WaitFor(reservationMigrationService);

var gateway = builder.AddProject<Projects.Cinema_Gateway>("gateway")
    .WithReference(management)
    .WaitFor(management)
    .WithReference(reservation)
    .WaitFor(reservation)
    .WithExternalHttpEndpoints();

builder
    .AddNpmApp("cinema-web", "../Cinema.Web")
    .WithReference(gateway)
    .WaitFor(gateway)
    .WithHttpEndpoint(port: 54163, env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();