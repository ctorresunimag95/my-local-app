using System.Net.Http.Json;

namespace Cinema.Functional.Tests;

[Collection("AspireAppCollection")]
public class ManagamentTests
{
    private readonly AspireAppFixture _app;

    public ManagamentTests(AspireAppFixture app)
    {
        _app = app;
    }

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var movieDto = new PublishMovieRequest(
            "TestMovie"
            , "TestDescription"
            , "TestGenre"
            , "TestPosterUri"
            , DateTime.Now
        );
    
        // Act
        var response = await _app.GatewayClient.PostAsJsonAsync("/api/management/movies/publish", movieDto);
        response.EnsureSuccessStatusCode();
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadFromJsonAsync<PublishMovieResponse>();
        Assert.NotNull(responseBody);
    }
    
    public record PublishMovieRequest(
        string Name,
        string Description,
        string Genre,
        string PosterUri,
        DateTime ReleaseDate
    );

    public record PublishMovieResponse(
        Guid Id,
        string Name,
        string Description,
        string Genre,
        string PosterUri,
        DateTime ReleaseDate
    );
}