namespace Cinema.Reservation.Movies.MoviePublished;

public record MovieCreatedEvent(
    Guid Id,
    string Name,
    string Description,
    string Genre);