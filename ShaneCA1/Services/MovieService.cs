using System.Text.Json.Serialization;

namespace ShaneCA1;

public interface IMovieService
{// gets a list of popular movies from TMDB.
    Task<List<Movie>> GetPopularMoviesAsync(int page);
}

public class MoviePageJson
{
    [JsonPropertyName("results")] public List<MovieJson> Results { get; set; } = new List<MovieJson>();
}

public class MovieJson
{// find the properties we need from the TMDB json to populater the variables.
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("release_date")] public string? ReleaseDate { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
}

public class MovieService : IMovieService
{
    private readonly HttpClient http;
    private readonly string apiKey;

    public MovieService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.apiKey = configuration["TmdbApiKey"] ?? "";
    }

    public async Task<List<Movie>> GetPopularMoviesAsync(int page)
    {
        MoviePageJson data = await ApiHelper.GetAsync<MoviePageJson>(http, $"movie/popular?page={page}&api_key={apiKey}", "TMDB");
        
        List<Movie> movies = new List<Movie>();
        foreach (MovieJson item in data.Results)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                continue; // Skip any movie with no title.
            }
            
            // API returns year as "2026-10-15" or "" if unknown. I only want the year.
            int year = 0;
            if (item.ReleaseDate != null && item.ReleaseDate.Length >= 4) 
            {// Parse the release year and only get the year.
                int.TryParse(item.ReleaseDate.Substring(0, 4), out year);
            }

            // Some films have no poster. In that case the poster URL stays empty.
            string posterUrl = "";
            if (!string.IsNullOrWhiteSpace(item.PosterPath))
            {
                posterUrl = "https://image.tmdb.org/t/p/w500" + item.PosterPath;
            }

            Movie movie = new Movie(
                item.Id,
                item.Title,
                year,
                item.Overview ?? "",
                posterUrl,
                item.VoteAverage,
                item.VoteCount,
                item.Popularity);

            movies.Add(movie);
        }

        return movies;
    }
}