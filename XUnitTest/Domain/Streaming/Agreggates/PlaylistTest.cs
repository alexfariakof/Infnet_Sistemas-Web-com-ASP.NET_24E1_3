using Domain.Streaming.Agreggates;

namespace Domain.Streaming;
public class PlaylistTest
{
    [Fact]
    public void Should_Set_Properties_Correctly_Playlist()
    {
        // Arrange
        var fakePlaylist = MockPlaylist.Instance.GetFaker();

        // Act
        var playlist = new Playlist
        {
            Id = fakePlaylist.Id,
            Name = fakePlaylist.Name,
            Flats = fakePlaylist.Flats,
            Musics = fakePlaylist.Musics
        };

        // Assert
        Assert.Equal(fakePlaylist.Id, playlist.Id);
        Assert.Equal(fakePlaylist.Name, playlist.Name);
        Assert.Equal(fakePlaylist.Flats, playlist.Flats);
        Assert.Equal(fakePlaylist.Musics, playlist.Musics);
    }

    [Fact]
    public void Should_Add_Music_Correctly_Playlist()
    {
        // Arrange
        var playlist = new Playlist();
        var fakeMusic = MockMusic.Instance.GetFaker();
        var fakeMusicList = MockMusic.Instance.GetListFaker(2);

        // Act
        playlist.Musics.Add(fakeMusic);
        foreach(var music in fakeMusicList)
            playlist.Musics.Add(music);

        // Assert
        Assert.Single(playlist.Musics, fakeMusic);
        Assert.True(fakeMusicList.Count < playlist.Musics.Count); 
    }
}