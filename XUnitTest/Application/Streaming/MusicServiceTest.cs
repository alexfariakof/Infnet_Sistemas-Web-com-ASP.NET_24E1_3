using Moq;
using System.Linq.Expressions;
using Application.Account.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
public class MusicServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Music>> musicRepositoryMock;
    private readonly MusicService musicService;
    private readonly List<Music> mockMusicList = MockMusic.Instance.GetListFaker(3);

    public MusicServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        musicRepositoryMock = new Mock<IRepository<Music>>();
        musicService = new MusicService(mapperMock.Object, musicRepositoryMock.Object);
    }

    [Fact]
    public void Create_MusicService_Successfully()
    {
        // Arrange
        var mockMusicService = MockMusic.Instance.GetFaker();
        var musicDto = new MusicDto
        {
            Id = mockMusicService.Id,
            Name = mockMusicService.Name,
            AlbumId = Guid.NewGuid(),            
            Duration = mockMusicService.Duration,
            FlatId = Guid.NewGuid()
        };

        musicRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Music, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Music>(It.IsAny<MusicDto>())).Returns(mockMusicService);
        mapperMock.Setup(mapper => mapper.Map<MusicDto>(It.IsAny<Music>())).Returns(musicDto);

        // Act
        var result = musicService.Create(musicDto);

        // Assert
        musicRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Music, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Music>(It.IsAny<MusicDto>()), Times.Once);
        musicRepositoryMock.Verify(repo => repo.Save(It.IsAny<Music>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(musicDto.Id, result.Id);
        Assert.Equal(musicDto.Name, result.Name);
        Assert.Equal(musicDto.AlbumId, result.AlbumId);
        Assert.Equal(musicDto.Duration, result.Duration);
        Assert.Equal(musicDto.FlatId, result.FlatId);
    }

    [Fact]
    public void FindAll_MusicServices_Successfully()
    {
        // Arrange
        var musicDtos = MockMusic.Instance.GetDtoListFromMusicList(mockMusicList);
        var userId = mockMusicList.First().Id;
        mapperMock.Setup(mapper => mapper.Map<List<MusicDto>>(It.IsAny<List<Music>>())).Returns(musicDtos.FindAll(c => c.Id.Equals(userId)));

        // Act
        var result = musicService.FindAll(userId);

        // Assert
        musicRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<MusicDto>>(It.IsAny<List<Music>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockMusicList.FindAll(c => c.Id.Equals(userId)).Count, result.Count);
        Assert.All(result, musicDto => Assert.Equal(userId, musicDto.Id));
    }

    [Fact]
    public void FindById_MusicService_Successfully()
    {
        // Arrange
        var mockMusicService = mockMusicList.Last();
        var musicId = mockMusicService.Id;
        var musicDto = MockMusic.Instance.GetDtoFromMusic(mockMusicService);

        musicRepositoryMock.Setup(repo => repo.GetById(musicId)).Returns(mockMusicService);
        mapperMock.Setup(mapper => mapper.Map<MusicDto>(It.IsAny<Music>())).Returns(musicDto);

        // Act
        var result = musicService.FindById(musicId);

        // Assert
        musicRepositoryMock.Verify(repo => repo.GetById(musicId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<MusicDto>(mockMusicService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockMusicService.Name, result.Name);
    }

    [Fact]
    public void Update_MusicService_Successfully()
    {
        // Arrange
        var mockMusicService = MockMusic.Instance.GetFaker();
        var musicDto = MockMusic.Instance.GetDtoFromMusic(mockMusicService);

        mapperMock.Setup(mapper => mapper.Map<Music>(It.IsAny<MusicDto>())).Returns(mockMusicService);
        musicRepositoryMock.Setup(repo => repo.Update(mockMusicService));
        mapperMock.Setup(mapper => mapper.Map<MusicDto>(It.IsAny<Music>())).Returns(musicDto);

        // Act
        var result = musicService.Update(musicDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Music>(It.IsAny<MusicDto>()), Times.Once);
        musicRepositoryMock.Verify(repo => repo.Update(mockMusicService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<MusicDto>(It.IsAny<Music>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(musicDto.Name, result.Name);
    }

    [Fact]
    public void Delete_MusicService_Successfully()
    {
        // Arrange
        var mockMusicService = MockMusic.Instance.GetFaker();
        var musicDto = MockMusic.Instance.GetDtoFromMusic(mockMusicService);

        mapperMock.Setup(mapper => mapper.Map<Music>(It.IsAny<MusicDto>())).Returns(mockMusicService);
        musicRepositoryMock.Setup(repo => repo.Delete(mockMusicService));

        // Act
        var result = musicService.Delete(musicDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Music>(It.IsAny<MusicDto>()), Times.Once);
        musicRepositoryMock.Verify(repo => repo.Delete(mockMusicService), Times.Once);

        Assert.True(result);
    }
}