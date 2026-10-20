namespace ShaneCA1.UnitTests;

public class CountryTests
{
    [Test]
    public void Country_Constructor_SetsPropertiesCorrectly()
    {
        var country = new Country("IE", "Ireland", "Dublin", "Europe", 5127000, "flag.png");

        Assert.That(country.Name, Is.EqualTo("Ireland"));
        Assert.That(country.Code, Is.EqualTo("IE"));
        Assert.That(country.Population, Is.EqualTo(5127000));
    }

    [Test]
    public void Country_ToString_ReturnsName()
    {
        var country = new Country("FR", "France", "Paris", "Europe", 67000000, "flag.png");

        Assert.That(country.ToString(), Is.EqualTo("France"));
    }
}