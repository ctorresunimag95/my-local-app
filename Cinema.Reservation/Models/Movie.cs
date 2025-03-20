namespace Cinema.Reservation.Models;

public class Movie
{
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public string Genre { get; }

    public Movie(Guid id, string name, string description, string genre)
    {
        Id = id;
        Name = name;
        Description = description;
        Genre = genre;
    }
}