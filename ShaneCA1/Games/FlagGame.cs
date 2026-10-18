namespace ShaneCA1;

public class FlagGame
{
    private readonly ICountryService countryService;
    private List<Country> allCountries = new List<Country>(); // used for searching
    private List<Country> rounds = new List<Country>();
    private int currentIndex = 0;
    private Random random = new Random();
    
    public int Score { get; private set; }

    public int TotalQuestions
    {
        get { return rounds.Count; }
    }
    
    public bool IsFinished
    {
        get { return currentIndex >= rounds.Count; }
    }
    
    // The country for the current round. null if the game is finished.
    public Country CurrentCountry
    {
        get { return IsFinished ? null : rounds[currentIndex]; }
    }
    
    public FlagGame(ICountryService countryService)
    {
        this.countryService = countryService;
    }
    
    public async Task StartAsync(int numberOfQuestions = 10)
    {
        Score = 0;
        currentIndex = 0;
        
        allCountries = await countryService.GetCountriesAsync();
        
        // Pick 10 random countries that have a flag.
        rounds = allCountries
            .Where(c => c.FlagUrl != "")
            .OrderBy(c => random.Next())
            .Take(numberOfQuestions)
            .ToList();

        if (rounds.Count == 0)
        {
            throw new Exception("No flags found.");
        }
    }

    // Return 6 country names that match the search text.
    public List<string> Search(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new List<string>();
        }

        text = text.Trim();
        return allCountries
            .Where(c => c.Name.Contains(text, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ThenBy(c => c.Name)
            .Select(c => c.Name)
            .Take(6)
            .ToList();
    }
    
    // True if the text matches the current country.
    public bool IsValidCountry(string name)
    {
        return allCountries.Any(c => string.Equals(c.Name, name.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public bool Answer(string guess)
    {
        if (IsFinished)
        {
            throw new InvalidOperationException("The game is already finished.");
        }
 
        bool correct = string.Equals(guess.Trim(), rounds[currentIndex].Name, StringComparison.OrdinalIgnoreCase);
        if (correct)
        {
            Score++;
        }
 
        currentIndex++;
        return correct;
    }
}