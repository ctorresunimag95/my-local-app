namespace Cinema.Management.Movies.PublishMovie;

public record MovieCreatedEvent(
    Guid Id,
    string Name,
    string Description,
    string Genre);