namespace ShaneCA1;

public class Country
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Capital { get; set; }
    public string Region { get; set; }
    public long Population { get; set; }
    public string FlagUrl { get; set; }

    public Country(string code, string name, string capital, string region, long population, string flagUrl)
    {
        Code = code;
        Name = name;
        Capital = capital;
        Region = region;
        Population = population;
        FlagUrl = flagUrl;
    }

    public Country()
    {
        Code = string.Empty;
        Name = "Unkown";
        Capital = "Unkown";
        Region = "Unkown";
        Population = 0;
        FlagUrl = "";
    }

    public override string ToString()
    {
        return Name;
    }
}