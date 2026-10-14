namespace ShaneCA1;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public string Overview { get; set; }
    public string PosterUrl { get; set; }
    public double Rating { get; set; }
    public int VoteCount { get; set; }
    public double Popularity { get; set; }

    public Movie(int id, string title, int releaseYear, string overview, string posterUrl, double rating, int voteCount, double popularity)
    {
        Id = id;
        Title = title;
        ReleaseYear = releaseYear;
        Overview = overview;
        PosterUrl = posterUrl;
        Rating = rating;
        VoteCount = voteCount;
        Popularity = popularity;
    }

    public Movie()
    {
        Id = 0;
        Title = "Unknown";
        ReleaseYear = 0;
        Overview = "Unknown";
        PosterUrl = "";
        Rating = 0.0;
        VoteCount = 0;
        Popularity = 0.0;
    }

    public override string ToString()
    {
        return $"{Title} ({ReleaseYear})";
    }
}