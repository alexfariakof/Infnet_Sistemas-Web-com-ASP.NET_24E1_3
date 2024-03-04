﻿using Domain.Streaming.Agreggates;
using __mock__;

namespace Domain.Streaming;
public class AlbumTest
{
    [Fact]
    public void Should_Set_Properties_Correctly_Album()
    {
        // Arrange
        var fakeAlbum = MockAlbum.Instance.GetFaker();

        // Act
        var album = new Album
        {
            Id = fakeAlbum.Id,
            Name = fakeAlbum.Name,
            Musics = fakeAlbum.Musics
        };

        // Assert
        Assert.Equal(fakeAlbum.Id, album.Id);
        Assert.Equal(fakeAlbum.Name, album.Name);
        Assert.Equal(fakeAlbum.Musics, album.Musics);
    }

    [Fact]
    public void Should_Add_Music_Correctly_Album()
    {
        // Arrange
        var album = new Album();
        var fakeMusic = MockMusic.GetFaker();
        var fakeMusicList = MockMusic.GetListFaker(2);

        // Act
        album.AddMusic(fakeMusic);
        album.AddMusic(fakeMusicList);

        // Assert
        Assert.Single(album.Musics, fakeMusic);
        Assert.True(fakeMusicList.Count < album.Musics.Count);
    }

    [Fact]
    public void Should_Add_Music_Personal_Correctly_Album()
    {
        // Arrange
        var album = new Album();
        var fakeMusic = MockPlaylistPersonal.GetFaker();
        var fakeMusicList = MockPlaylistPersonal.GetListFaker(2);

        // Act
        album.AddMusic(fakeMusic);
        album.AddMusic(fakeMusicList);

        // Assert
        Assert.Single(album.MusicPersonal, fakeMusic);
        Assert.True(fakeMusicList.Count < album.MusicPersonal.Count);
    }
}