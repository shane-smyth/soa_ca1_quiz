namespace ShaneCA1.UnitTests;

public class MovieTests
{
    [Test]
    public void Movie_Constructor_SetsPropertiesCorrectly()
    {
        var movie = new Movie(1, "The Godfather", 1972, "Crime", "https://media.themoviedb.org/t/p/w300_and_h450_face/3bhkrj58Vtu7enYsRolD1fZdja1.jpg", 9.2, 1000, 3.5);
        
        Assert.That(movie.Title, Is.EqualTo("The Godfather"));
        Assert.That(movie.ReleaseYear, Is.EqualTo(1972));
        Assert.That(movie.Rating, Is.EqualTo(9.2));
    }
    
    [Test]
    public void Movie_ToString_FormatsCorrectly()
    {
        var movie = new Movie(1, "Inception", 2010, "Overview", "https://media.themoviedb.org/t/p/w300_and_h450_face/xlaY2zyzMfkhk0HSC5VUwzoZPU1.jpg", 8.8, 2000, 95.0);

        Assert.That(movie.ToString(), Is.EqualTo("Inception (2010)"));
    }
}