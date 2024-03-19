using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account;
public class PlaylistProfileTest
{
    [Fact]
    public void Map_PlaylistDto_To_Playlist_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PlaylistProfile>();
        }));

        var mockPlaylist = MockPlaylist.Instance.GetFaker();
        var playlistDto = MockPlaylist.Instance.GetDtoFromPlaylist(mockPlaylist);

        // Act
        var playlist = mapper.Map<Playlist>(playlistDto);

        // Assert
        Assert.NotNull(playlist);
        Assert.Equal(playlistDto.Id, playlist.Id);
        Assert.Equal(playlistDto.Name, playlist.Name);
        Assert.NotNull(playlist.Musics);
        Assert.Empty(playlist.Musics);
    }

    [Fact]
    public void Map_Playlist_To_PlaylistDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PlaylistProfile>();
        }));

        var playlist = MockPlaylist.Instance.GetFaker();
        playlist.Musics.Add(MockMusic.Instance.GetFaker());

        // Act
        var playlistDto = mapper.Map<PlaylistDto>(playlist);

        // Assert
        Assert.NotNull(playlistDto);
        Assert.Equal(playlist.Id, playlistDto.Id);
        Assert.Equal(playlist.Name, playlistDto.Name);
        Assert.NotNull(playlistDto.Musics);
        Assert.Equal(playlist.Musics.Count, playlistDto.Musics.Count);
        Assert.Equal(playlist.Musics.First().Id, playlistDto.Musics.First().Id);
    }
}
