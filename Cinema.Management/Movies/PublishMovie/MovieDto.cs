namespace Cinema.Management.Movies.PublishMovie;

public record MovieDto(
    string Name,
    string Description,
    string Genre,
    string Producer,
    DateTime ReleaseDate
);