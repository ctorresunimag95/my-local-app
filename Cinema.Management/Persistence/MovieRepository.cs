using Cinema.Management.Models;
using Microsoft.Azure.Cosmos;

namespace Cinema.Management.Persistence;

internal sealed class MovieRepository
{
    private readonly Container _container;

    public MovieRepository(CosmosClient cosmosClient)
    {
        _container = cosmosClient.GetContainer("Cinema", "Movies");
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        var response = await _container.UpsertItemAsync(movie,
            cancellationToken: cancellationToken);
    }
}