using Moq;
using System.Linq.Expressions;
using AutoMapper;
using Repository;
using Domain.Account.Agreggates;
using Application.Account.Dto;

namespace Application.Account;
public class PlaylistPersonalServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<PlaylistPersonal>> playlistPersonalRepositoryMock;
    private readonly PlaylistPersonalService playlistPersonalService;
    private readonly List<PlaylistPersonal> mockPlaylistPersonalList = MockPlaylistPersonal.Instance.GetListFaker(3);

    public PlaylistPersonalServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        playlistPersonalRepositoryMock = new Mock<IRepository<PlaylistPersonal>>();
        playlistPersonalService = new PlaylistPersonalService(mapperMock.Object, playlistPersonalRepositoryMock.Object);
    }

    [Fact]
    public void Create_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = MockPlaylistPersonal.Instance.GetFaker();
        var PlaylistPersonalDto = new PlaylistPersonalDto
        {
            Id = mockPlaylistPersonalService.Id,
            Name = mockPlaylistPersonalService.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(MockMusic.Instance.GetListFaker(3))

        };

        playlistPersonalRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>())).Returns(mockPlaylistPersonalService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(PlaylistPersonalDto);

        // Act
        var result = playlistPersonalService.Create(PlaylistPersonalDto);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>()), Times.Once);
        playlistPersonalRepositoryMock.Verify(repo => repo.Save(It.IsAny<PlaylistPersonal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(PlaylistPersonalDto.Id, result.Id);
        Assert.Equal(PlaylistPersonalDto.Name, result.Name);
        Assert.NotNull(result.Musics);
        Assert.Equal(PlaylistPersonalDto.Musics, result.Musics);        
    }

    [Fact]
    public void FindAll_PlaylistPersonalServices_Successfully()
    {
        // Arrange
        var PlaylistPersonalDtos = MockPlaylistPersonal.Instance.GetDtoListFromPlaylistPersonalList(mockPlaylistPersonalList);
        var userId = mockPlaylistPersonalList.First().Id;
        mapperMock.Setup(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<List<PlaylistPersonal>>())).Returns(PlaylistPersonalDtos.FindAll(c => c.Id.Equals(userId)));

        // Act
        var result = playlistPersonalService.FindAll(userId);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<List<PlaylistPersonal>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockPlaylistPersonalList.FindAll(c => c.Id.Equals(userId)).Count, result.Count);
        Assert.All(result, PlaylistPersonalDto => Assert.Equal(userId, PlaylistPersonalDto.Id));
    }

    [Fact]
    public void FindById_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = mockPlaylistPersonalList.Last();
        var PlaylistPersonalId = mockPlaylistPersonalService.Id;
        var PlaylistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPlaylistPersonalService);

        playlistPersonalRepositoryMock.Setup(repo => repo.GetById(PlaylistPersonalId)).Returns(mockPlaylistPersonalService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(PlaylistPersonalDto);

        // Act
        var result = playlistPersonalService.FindById(PlaylistPersonalId);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.GetById(PlaylistPersonalId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonalDto>(mockPlaylistPersonalService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockPlaylistPersonalService.Name, result.Name);
    }

    [Fact]
    public void Update_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = MockPlaylistPersonal.Instance.GetFaker();
        var PlaylistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPlaylistPersonalService);

        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>())).Returns(mockPlaylistPersonalService);
        playlistPersonalRepositoryMock.Setup(repo => repo.Update(mockPlaylistPersonalService));
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(PlaylistPersonalDto);

        // Act
        var result = playlistPersonalService.Update(PlaylistPersonalDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>()), Times.Once);
        playlistPersonalRepositoryMock.Verify(repo => repo.Update(mockPlaylistPersonalService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(PlaylistPersonalDto.Name, result.Name);
    }

    [Fact]
    public void Delete_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = MockPlaylistPersonal.Instance.GetFaker();
        var PlaylistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPlaylistPersonalService);

        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>())).Returns(mockPlaylistPersonalService);
        playlistPersonalRepositoryMock.Setup(repo => repo.Delete(mockPlaylistPersonalService));

        // Act
        var result = playlistPersonalService.Delete(PlaylistPersonalDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>()), Times.Once);
        playlistPersonalRepositoryMock.Verify(repo => repo.Delete(mockPlaylistPersonalService), Times.Once);

        Assert.True(result);
    }
}