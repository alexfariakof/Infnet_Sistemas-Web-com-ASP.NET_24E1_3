using Moq;
using System.Linq.Expressions;
using Application.Streaming.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class PlaylistServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Playlist>> playlistRepositoryMock;
    private readonly PlaylistService playlistService;
    private readonly List<Playlist> mockPlaylistList = MockPlaylist.Instance.GetListFaker(3);

    public PlaylistServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        playlistRepositoryMock = new Mock<IRepository<Playlist>>();
        playlistService = new PlaylistService(mapperMock.Object, playlistRepositoryMock.Object);
    }

    [Fact]
    public void Create_PlaylistService_Successfully()
    {
        // Arrange
        var mockPlaylistService = MockPlaylist.Instance.GetFaker();
        var playlistDto = new PlaylistDto
        {
            Id = mockPlaylistService.Id,
            Name = mockPlaylistService.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(MockMusic.Instance.GetListFaker(3))

        };

        playlistRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Playlist, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>())).Returns(mockPlaylistService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistDto>(It.IsAny<Playlist>())).Returns(playlistDto);

        // Act
        var result = playlistService.Create(playlistDto);

        // Assert
        playlistRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Playlist, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>()), Times.Once);
        playlistRepositoryMock.Verify(repo => repo.Save(It.IsAny<Playlist>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(playlistDto.Id, result.Id);
        Assert.Equal(playlistDto.Name, result.Name);
        Assert.NotNull(result.Musics);
        Assert.Equal(playlistDto.Musics, result.Musics);        
    }

    [Fact]
    public void FindAll_PlaylistServices_Successfully()
    {
        // Arrange
        var playlistDtos = MockPlaylist.Instance.GetDtoListFromPlaylistList(mockPlaylistList);
        var userId = mockPlaylistList.First().Id;
        mapperMock.Setup(mapper => mapper.Map<List<PlaylistDto>>(It.IsAny<IEnumerable<Playlist>>())).Returns(playlistDtos);

        // Act
        var result = playlistService.FindAll();

        // Assert
        playlistRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<PlaylistDto>>(It.IsAny<IEnumerable<Playlist>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockPlaylistList.Count, result.Count);
    }

    [Fact]
    public void FindById_PlaylistService_Successfully()
    {
        // Arrange
        var mockPlaylistService = mockPlaylistList.Last();
        var playlistId = mockPlaylistService.Id;
        var playlistDto = MockPlaylist.Instance.GetDtoFromPlaylist(mockPlaylistService);

        playlistRepositoryMock.Setup(repo => repo.GetById(playlistId)).Returns(mockPlaylistService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistDto>(It.IsAny<Playlist>())).Returns(playlistDto);

        // Act
        var result = playlistService.FindById(playlistId);

        // Assert
        playlistRepositoryMock.Verify(repo => repo.GetById(playlistId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistDto>(mockPlaylistService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockPlaylistService.Name, result.Name);
    }

    [Fact]
    public void Update_PlaylistService_Successfully()
    {
        // Arrange
        var mockPlaylistService = MockPlaylist.Instance.GetFaker();
        var playlistDto = MockPlaylist.Instance.GetDtoFromPlaylist(mockPlaylistService);

        mapperMock.Setup(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>())).Returns(mockPlaylistService);
        playlistRepositoryMock.Setup(repo => repo.Update(mockPlaylistService));
        mapperMock.Setup(mapper => mapper.Map<PlaylistDto>(It.IsAny<Playlist>())).Returns(playlistDto);

        // Act
        var result = playlistService.Update(playlistDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>()), Times.Once);
        playlistRepositoryMock.Verify(repo => repo.Update(mockPlaylistService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistDto>(It.IsAny<Playlist>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(playlistDto.Name, result.Name);
    }

    [Fact]
    public void Delete_PlaylistService_Successfully()
    {
        // Arrange
        var mockPlaylistService = MockPlaylist.Instance.GetFaker();
        var playlistDto = MockPlaylist.Instance.GetDtoFromPlaylist(mockPlaylistService);

        mapperMock.Setup(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>())).Returns(mockPlaylistService);
        playlistRepositoryMock.Setup(repo => repo.Delete(mockPlaylistService));

        // Act
        var result = playlistService.Delete(playlistDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Playlist>(It.IsAny<PlaylistDto>()), Times.Once);
        playlistRepositoryMock.Verify(repo => repo.Delete(mockPlaylistService), Times.Once);

        Assert.True(result);
    }
}