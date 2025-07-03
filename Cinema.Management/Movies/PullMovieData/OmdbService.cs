namespace Cinema.Management.Movies.PullMovieData;

public sealed class OmdbService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OmdbService> _logger;
    private readonly IConfiguration _configuration;

    public OmdbService(HttpClient httpClient, ILogger<OmdbService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<MovieResponseDto?> GetMovieAsync(string title, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<MovieResponseDto>(
                $"?apiKey={_configuration.GetValue<string>("omdb:apiKey")}&t={title}", cancellationToken);

            return !response!.IsSuccess ? null : response!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting movie");
            return null;
        }
    }
}

public record MovieResponseDto(
    string Response,
    string? Title,
    string? Year,
    string? Released,
    string? Genre,
    string? Poster,
    string? Plot)
{
    public bool IsSuccess => Response.Equals("true", StringComparison.OrdinalIgnoreCase);
}