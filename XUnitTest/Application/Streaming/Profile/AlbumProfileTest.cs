using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account;
public class AlbumProfileTest
{
    [Fact]
    public void Map_AlbumDto_To_Album_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AlbumProfile>();
        }));

        var musics = MockMusic.Instance.GetListFaker(5);
        var mockAlbum = MockAlbum.Instance.GetFaker(musics);
        var albumDto = MockAlbum.Instance.GetDtoFromAlbum(mockAlbum);

        // Act
        var album = mapper.Map<Album>(albumDto);

        // Assert
        Assert.NotNull(album);
        Assert.Equal(albumDto.Id, album.Id);
        Assert.Equal(albumDto.Name, album.Name);
        Assert.NotNull(album.Musics);
        Assert.NotEmpty(album.Musics);
    }

    [Fact]
    public void Map_Album_To_AlbumDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AlbumProfile>();
        }));

        var musics = MockMusic.Instance.GetListFaker(5);
        var album = MockAlbum.Instance.GetFaker(musics);
        album.AddMusic(MockMusic.Instance.GetListFaker(3));

        // Act
        var albumDto = mapper.Map<AlbumDto>(album);

        // Assert
        Assert.NotNull(albumDto);
        Assert.Equal(album.Id, albumDto.Id);
        Assert.Equal(album.Name, albumDto.Name);
        Assert.NotNull(albumDto.Musics);
        Assert.Equal(album.Musics.Count, albumDto.Musics.Count);
        Assert.Equal(album.Musics.First().Id, albumDto.Musics.First().Id);
    }
}
