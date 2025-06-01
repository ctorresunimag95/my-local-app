using Microsoft.Azure.Cosmos;

namespace Cinema.Management.Persistence;

internal static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(new CosmosClient(
            connectionString: configuration.GetConnectionString("cosmosDb"),
            clientOptions: new()
            {
                LimitToEndpoint = true,
                HttpClientFactory = () => new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }),
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            }));

        services.AddScoped<MovieRepository>();

        return services;
    }

    public static async Task CreateDbIfNotExistsAsync(this IApplicationBuilder app)
    {
        var cosmosClient = app.ApplicationServices.GetRequiredService<CosmosClient>();

        var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
            id: "Cinema",
            throughput: 400
        );

        await database.Database.CreateContainerIfNotExistsAsync(
            id: "Movies", 
            partitionKeyPath: "/genre"
        );
    }
}