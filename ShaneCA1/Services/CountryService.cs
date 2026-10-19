namespace ShaneCA1;
using System.Text.Json.Serialization;

public interface ICountryService
{
    Task<List<Country>> GetCountriesAsync();
}

public class CountryPageJson
{
    [JsonPropertyName("data")] public CountryDataJson? Data { get; set; }
}

public class CountryDataJson
{
    [JsonPropertyName("objects")] public List<CountryJson> Objects { get; set; } = new List<CountryJson>();
    [JsonPropertyName("meta")] public CountryMetaJson? Meta { get; set; }
}

public class CountryMetaJson
{
    [JsonPropertyName("more")] public bool More { get; set; }
}

public class CountryJson
{
    [JsonPropertyName("names")] public CountryNamesJson? Names { get; set; }
    [JsonPropertyName("codes")] public CountryCodesJson? Codes { get; set; }
    [JsonPropertyName("region")] public string? Region { get; set; }
    [JsonPropertyName("population")] public double Population { get; set; }
    [JsonPropertyName("flag")] public CountryFlagJson? Flag { get; set; }
}

public class CountryNamesJson
{
    [JsonPropertyName("common")] public string? Common { get; set; }
}

public class CountryCodesJson
{
    [JsonPropertyName("alpha_2")] public string? Alpha2 { get; set; }
}

public class CountryFlagJson
{
    [JsonPropertyName("url_png")] public string? UrlPng { get; set; }
}

public class CountryService : ICountryService
{
    private const int PageSize = 100; // Free version of API only allows 100 countries per request.

    private readonly HttpClient http;
    
    private static List<Country>? cachedCountries;

    public CountryService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        if (cachedCountries != null)
        {
            return cachedCountries;
        }

        List<Country> countries = new List<Country>();
        int offset = 0;
        bool more = true;

        // There are about 250 countries but only 100 come back at a time, so ask for one page at a time.
        while (more)
        {
            string url = $"countries/v5?limit={PageSize}&offset={offset}" +
                         "&response_fields=names.common,codes.alpha_2,region,population,flag.url_png";

            CountryPageJson page = await ApiHelper.GetAsync<CountryPageJson>(http, url, "REST Countries");

            if (page.Data == null)
            {
                throw new ApiException("REST Countries sent back an unexpected response.");
            }

            foreach (CountryJson item in page.Data.Objects)
            {
                string code = item.Codes?.Alpha2 ?? "";
                string name = item.Names?.Common ?? "";

                if (code == "" || name == "")
                {
                    continue;   // skip any country with no code or name
                }

                // If the API did not give a flag, build the address from the free flag CDN.
                string flagUrl = item.Flag?.UrlPng ?? "";
                if (flagUrl == "")
                {
                    flagUrl = $"https://flags.restcountries.com/v5/w320/{code.ToLower()}.png";
                }

                countries.Add(new Country
                {
                    Code = code,
                    Name = name,
                    Capital = "N/A",   // not requested, the flag game does not need it
                    Region = item.Region ?? "N/A",
                    Population = (long)item.Population,
                    FlagUrl = flagUrl
                });
            }

            more = page.Data.Meta != null && page.Data.Meta.More && page.Data.Objects.Count > 0;
            offset += PageSize;
        }

        if (countries.Count == 0)
        {
            throw new ApiException("REST Countries did not send back any countries.");
        }

        cachedCountries = countries;
        return countries;
    }
}