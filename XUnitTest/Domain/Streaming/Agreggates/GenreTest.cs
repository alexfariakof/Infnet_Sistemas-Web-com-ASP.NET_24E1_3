using Domain.Streaming.Agreggates;

namespace Domain.Streaming;
public class GenreTest
{
    [Fact]
    public void Should_Set_Properties_Correctly_Genre()
    {
        // Arrange
        var fakeGenre = MockGenre.Instance.GetFaker();

        // Act
        var genre = new Genre
        {
            Id = fakeGenre.Id,
            Name = fakeGenre.Name,
            Albums = fakeGenre.Albums,
            Bands = fakeGenre.Bands,
            Musics = fakeGenre.Musics,
            Playlists = fakeGenre.Playlists
        };

        // Assert
        Assert.Equal(fakeGenre.Id, genre.Id);
        Assert.Equal(fakeGenre.Name, genre.Name);
        Assert.Equal(fakeGenre.Albums, genre.Albums);
        Assert.Equal(fakeGenre.Bands, genre.Bands);
        Assert.Equal(fakeGenre.Musics, genre.Musics);
        Assert.Equal(fakeGenre.Playlists, genre.Playlists);

    }

}