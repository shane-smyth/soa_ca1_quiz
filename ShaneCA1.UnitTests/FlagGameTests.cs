namespace ShaneCA1.UnitTests;

public class FlagGameTests
{
    private class TestCountryService : ICountryService
    {
        public Task<List<Country>> GetCountriesAsync()
        {
            return Task.FromResult(new List<Country>
            {
                new("IE", "Ireland", "Dublin", "Europe", 5127000, "flag1.png"),
                new("FR", "France", "Paris", "Europe", 67000000, "flag2.png"),
                new("DE", "Germany", "Berlin", "Europe", 83000000, "flag3.png")
            });
        }
    }

    [Test]
    public async Task FlagGame_Answer_IncrementsScoreOnCorrectAnswer()
    {
        var game = new FlagGame(new TestCountryService());
        await game.StartAsync(1);
        
        string countryName = game.CurrentCountry.Name;
        bool result = game.Answer(countryName);

        Assert.That(result, Is.True);
        Assert.That(game.Score, Is.EqualTo(1));
    }

    [Test]
    public async Task FlagGame_Answer_ReturnsFalseOnIncorrectAnswer()
    {
        var game = new FlagGame(new TestCountryService());
        await game.StartAsync(1);

        bool result = game.Answer("InvalidCountry");

        Assert.That(result, Is.False);
        Assert.That(game.Score, Is.EqualTo(0));
    }
}