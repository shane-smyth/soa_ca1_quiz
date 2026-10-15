namespace ShaneCA1;

public class MovieGame
{
    private readonly IMovieService movieService;
    private List<Movie> movies = new List<Movie>(); // round 0 uses movies 0 and 1, round 1 uses 2 and 3...
    private int currentRound = 0;
    private Random random = new Random();

    public int Score { get; private set; }

    public int TotalRounds
    {
        get { return movies.Count / 2; }
    }

    public bool IsFinished
    {
        get { return currentRound >= TotalRounds; }
    }

    public Movie? FirstMovie
    {
        get { return IsFinished ? null : movies[currentRound * 2]; }
    }

    public Movie? SecondMovie
    {
        get { return IsFinished ? null : movies[currentRound * 2 + 1]; }
    }

    public MovieGame(IMovieService movieService)
    {
        this.movieService = movieService;
    }

    public async Task StartAsync(int numberOfRounds = 10)
    {
        Score = 0;
        currentRound = 0;
        movies = new List<Movie>();

        // Each page has about 20 movies, so 2 pages is enough for 10 rounds.
        List<Movie> all = new List<Movie>();
        all.AddRange(await movieService.GetPopularMoviesAsync(1));
        all.AddRange(await movieService.GetPopularMoviesAsync(2));

        // Only use movies that have a poster and enough votes for the rating to be fair.
        List<Movie> usable = all
            .Where(m => m.PosterUrl != "" && m.VoteCount >= 100)
            .OrderBy(m => random.Next())
            .ToList();

        // Pair the movies up. If two movies have exactly the same rating, skip that pair.
        for (int i = 0; i + 1 < usable.Count && movies.Count < numberOfRounds * 2; i += 2)
        {
            if (usable[i].Rating != usable[i + 1].Rating)
            {
                movies.Add(usable[i]);
                movies.Add(usable[i + 1]);
            }
        }

        if (movies.Count == 0)
        {
            throw new ApiException("Not enough movies to make a round.");
        }
    }

    // The player guesses higher (true) or lower (false). Returns true if they were right.
    public bool Guess(bool guessedHigher)
    {
        if (IsFinished)
        {
            throw new InvalidOperationException("The game is already finished.");
        }

        bool secondIsHigher = SecondMovie!.Rating > FirstMovie!.Rating;
        bool correct = guessedHigher == secondIsHigher;
        if (correct)
        {
            Score++;
        }

        currentRound++;
        return correct;
    }
}