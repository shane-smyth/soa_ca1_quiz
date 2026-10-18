namespace ShaneCA1;
using System.Text.Json.Serialization;

public interface ICountryService
{
    Task<List<Country>> GetCountriesAsync();
}

public class CountryJson
{
    [JsonPropertyName("cca2")] public string? Code { get; set; }
    [JsonPropertyName("name")] public CountryNameJson? Name { get; set; }
    [JsonPropertyName("capital")] public List<string>? Capital { get; set; }
    [JsonPropertyName("region")] public string? Region { get; set; }
    [JsonPropertyName("population")] public long Population { get; set; }
    [JsonPropertyName("flags")] public CountryFlagsJson? Flags { get; set; }
}

public class CountryNameJson
{
    [JsonPropertyName("common")] public string? Common { get; set; }
}

public class CountryFlagsJson
{
    [JsonPropertyName("png")] public string? Png { get; set; }
}

public class CountryService : ICountryService
{
    private readonly HttpClient http;
    private readonly IConfiguration config;

    public CountryService(HttpClient http, IConfiguration config)
    {
        this.http = http;
        this.config = config;
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        string url = "v3.1/all?fields=name,cca2,capital,region,population,flags";
        var apiKey = config["RestCountriesApiKey"];
        
        if (!string.IsNullOrEmpty(apiKey))
        {
            url += $"&apiKey={apiKey}";
        }
        
        List<CountryJson> data = await ApiHelper.GetAsync<List<CountryJson>>(http, url, "RestCountries");

        List<Country> countries = new List<Country>();
        foreach (CountryJson item in data)
        {
            if (string.IsNullOrWhiteSpace(item.Code) || item.Name == null || string.IsNullOrWhiteSpace(item.Name.Common))
            {
                continue; // Skip any country with no code or name.
            }

            string capital = item.Capital != null && item.Capital.Count > 0 ? item.Capital[0] : "N/A";
            string region = item.Region ?? "N/A";
            long population = item.Population;
            string flagUrl = item.Flags?.Png ?? "";

            countries.Add(new Country
            {
                Code = item.Code,
                Name = item.Name.Common,
                Capital = capital,
                Region = region,
                Population = population,
                FlagUrl = flagUrl
            });
        }

        return countries;
    }
}