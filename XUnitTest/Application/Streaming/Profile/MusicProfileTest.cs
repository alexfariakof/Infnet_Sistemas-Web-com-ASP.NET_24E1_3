using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account;
public class MusicProfileTest
{
    [Fact]
    public void Map_MusicDto_To_Music_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MusicProfile>();
        }));

        var mockMusic = MockMusic.Instance.GetFaker();
        mockMusic.Playlists.Add(MockPlaylist.Instance.GetFaker());
        mockMusic.Playlists.First().Musics.Add(mockMusic);
        mockMusic.Album.AddMusic(mockMusic);
        var musicDto = MockMusic.Instance.GetDtoFromMusic(mockMusic);

        // Act
        var music = mapper.Map<Music>(musicDto);

        // Assert
        Assert.NotNull(music);
        Assert.Equal(musicDto.Id, music.Id);
        Assert.Equal(musicDto.Name, music.Name);
        Assert.NotNull(music.Playlists);
        Assert.Empty(music.Playlists);
    }

    [Fact]
    public void Map_Music_To_MusicDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MusicProfile>();
        }));

        var music = MockMusic.Instance.GetFaker();
        music.Playlists.Add(MockPlaylist.Instance.GetFaker());

        // Act
        var musicDto = mapper.Map<MusicDto>(music);

        // Assert
        Assert.NotNull(musicDto);
        Assert.Equal(music.Id, musicDto.Id);
        Assert.Equal(music.Name, musicDto.Name);
        Assert.NotNull(musicDto.AlbumId);
    }
}
