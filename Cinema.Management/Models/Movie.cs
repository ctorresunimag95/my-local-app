namespace Cinema.Management.Models;

public class Movie
{
    public Guid Id { get; }

    public string Name { get; }

    public string Description { get; }

    public string Genre { get; }

    public string PosterUri { get; }

    public DateTime ReleaseDate { get; }

    public Movie(string name, string description, string genre, string posterUri, DateTime releaseDate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Genre = genre;
        PosterUri = posterUri;
        ReleaseDate = releaseDate;
    }
}