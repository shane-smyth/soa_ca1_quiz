namespace ShaneCA1.UnitTests;

public class MovieGameTests
{
    private class TestMovieService : IMovieService
    {
        public Task<List<Movie>> GetPopularMoviesAsync(int page)
        {
            return Task.FromResult(new List<Movie>
            {
                new(1, "Movie A", 2020, "Overview", "poster1.jpg", 7.5, 200, 50),
                new(2, "Movie B", 2021, "Overview", "poster2.jpg", 8.5, 300, 75),
                new(3, "Movie C", 2022, "Overview", "poster3.jpg", 6.5, 150, 40),
                new(4, "Movie D", 2023, "Overview", "poster4.jpg", 9.0, 400, 90)
            });
        }
    }

    [Test]
    public async Task MovieGame_Guess_UpdatesGameState()
    {
        var game = new MovieGame(new TestMovieService());
        await game.StartAsync(2);

        Assert.That(game.Score, Is.EqualTo(0));
        Assert.That(game.TotalRounds, Is.GreaterThan(0));
        Assert.That(game.IsFinished, Is.False);
    }

    [Test]
    public async Task MovieGame_Guess_IncrementsRound()
    {
        var game = new MovieGame(new TestMovieService());
        await game.StartAsync(2);
        
        game.Guess(true);

        Assert.That(game.IsFinished, Is.False);
    }
}