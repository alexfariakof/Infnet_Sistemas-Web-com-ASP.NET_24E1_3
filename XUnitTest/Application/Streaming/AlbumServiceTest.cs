using Moq;
using System.Linq.Expressions;
using Application.Account.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
public class AlbumServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Album>> albumRepositoryMock;
    private readonly AlbumService albumService;
    private readonly List<Album> mockAlbumList = MockAlbum.Instance.GetListFaker(3);

    public AlbumServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        albumRepositoryMock = new Mock<IRepository<Album>>();
        albumService = new AlbumService(mapperMock.Object, albumRepositoryMock.Object);
    }

    [Fact]
    public void Create_AlbumService_Successfully()
    {
        // Arrange
        var mockAlbumService = MockAlbum.Instance.GetFaker();
        var albumDto = new AlbumDto
        {
            Id = mockAlbumService.Id,
            Name = mockAlbumService.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(MockMusic.Instance.GetListFaker(3))

        };

        albumRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Album, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>())).Returns(mockAlbumService);
        mapperMock.Setup(mapper => mapper.Map<AlbumDto>(It.IsAny<Album>())).Returns(albumDto);

        // Act
        var result = albumService.Create(albumDto);

        // Assert
        albumRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Album, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>()), Times.Once);
        albumRepositoryMock.Verify(repo => repo.Save(It.IsAny<Album>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(albumDto.Id, result.Id);
        Assert.Equal(albumDto.Name, result.Name);
        Assert.NotNull(result.Musics);
        Assert.Equal(albumDto.Musics, result.Musics);        
    }

    [Fact]
    public void FindAll_AlbumServices_Successfully()
    {
        // Arrange
        var albumDtos = MockAlbum.Instance.GetDtoListFromAlbumList(mockAlbumList);
        var userId = mockAlbumList.First().Id;
        mapperMock.Setup(mapper => mapper.Map<List<AlbumDto>>(It.IsAny<List<Album>>())).Returns(albumDtos.FindAll(c => c.Id.Equals(userId)));

        // Act
        var result = albumService.FindAll(userId);

        // Assert
        albumRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<AlbumDto>>(It.IsAny<List<Album>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockAlbumList.FindAll(c => c.Id.Equals(userId)).Count, result.Count);
        Assert.All(result, albumDto => Assert.Equal(userId, albumDto.Id));
    }

    [Fact]
    public void FindById_AlbumService_Successfully()
    {
        // Arrange
        var mockAlbumService = mockAlbumList.Last();
        var albumId = mockAlbumService.Id;
        var albumDto = MockAlbum.Instance.GetDtoFromAlbum(mockAlbumService);

        albumRepositoryMock.Setup(repo => repo.GetById(albumId)).Returns(mockAlbumService);
        mapperMock.Setup(mapper => mapper.Map<AlbumDto>(It.IsAny<Album>())).Returns(albumDto);

        // Act
        var result = albumService.FindById(albumId);

        // Assert
        albumRepositoryMock.Verify(repo => repo.GetById(albumId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<AlbumDto>(mockAlbumService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockAlbumService.Name, result.Name);
    }

    [Fact]
    public void Update_AlbumService_Successfully()
    {
        // Arrange
        var mockAlbumService = MockAlbum.Instance.GetFaker();
        var albumDto = MockAlbum.Instance.GetDtoFromAlbum(mockAlbumService);

        mapperMock.Setup(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>())).Returns(mockAlbumService);
        albumRepositoryMock.Setup(repo => repo.Update(mockAlbumService));
        mapperMock.Setup(mapper => mapper.Map<AlbumDto>(It.IsAny<Album>())).Returns(albumDto);

        // Act
        var result = albumService.Update(albumDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>()), Times.Once);
        albumRepositoryMock.Verify(repo => repo.Update(mockAlbumService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<AlbumDto>(It.IsAny<Album>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(albumDto.Name, result.Name);
    }

    [Fact]
    public void Delete_AlbumService_Successfully()
    {
        // Arrange
        var mockAlbumService = MockAlbum.Instance.GetFaker();
        var albumDto = MockAlbum.Instance.GetDtoFromAlbum(mockAlbumService);

        mapperMock.Setup(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>())).Returns(mockAlbumService);
        albumRepositoryMock.Setup(repo => repo.Delete(mockAlbumService));

        // Act
        var result = albumService.Delete(albumDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Album>(It.IsAny<AlbumDto>()), Times.Once);
        albumRepositoryMock.Verify(repo => repo.Delete(mockAlbumService), Times.Once);

        Assert.True(result);
    }
}