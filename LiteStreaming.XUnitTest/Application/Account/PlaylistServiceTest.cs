using Moq;
using System.Linq.Expressions;
using AutoMapper;
using Domain.Account.Agreggates;
using Application.Streaming.Dto;
using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class PlaylistPersonalServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<PlaylistPersonal>> playlistPersonalRepositoryMock;
    private readonly Mock<IRepository<Customer>> customerRepositoryMock;
    private readonly Mock<IRepository<Music>> musicRepositoryMock;

    private readonly PlaylistPersonalService playlistPersonalService;
    private readonly List<PlaylistPersonal> mockPlaylistPersonalList = MockPlaylistPersonal.Instance.GetListFaker(3);

    public PlaylistPersonalServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        playlistPersonalRepositoryMock = new Mock<IRepository<PlaylistPersonal>>();
        customerRepositoryMock = new Mock<IRepository<Customer>>();
        musicRepositoryMock = new Mock<IRepository<Music>>();
        playlistPersonalService = new PlaylistPersonalService(
            mapperMock.Object, 
            playlistPersonalRepositoryMock.Object,
            customerRepositoryMock.Object,
            musicRepositoryMock.Object);
    }

    [Fact]
    public void Create_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = MockPlaylistPersonal.Instance.GetFaker();
        var mockMusics = MockMusic.Instance.GetListFaker(3);
        var playlistPersonalDto = new PlaylistPersonalDto
        {
            Id = mockPlaylistPersonalService.Id,
            Name = mockPlaylistPersonalService.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(mockMusics)

        };

        customerRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(MockCustomer.Instance.GetListFaker(1));
        musicRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<Music, bool>>>())).Returns(mockMusics);
        playlistPersonalRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>())).Returns(mockPlaylistPersonalService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(playlistPersonalDto);

        // Act
        var result = playlistPersonalService.Create(playlistPersonalDto);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>()), Times.Once);
        playlistPersonalRepositoryMock.Verify(repo => repo.Save(It.IsAny<PlaylistPersonal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(playlistPersonalDto.Id, result.Id);
        Assert.Equal(playlistPersonalDto.Name, result.Name);
        Assert.NotNull(result.Musics);
        Assert.Equal(playlistPersonalDto.Musics, result.Musics);        
    }

    [Fact]
    public void FindAll_PlaylistPersonalServices_Successfully()
    {
        // Arrange
        var playlistPersonalDtos = MockPlaylistPersonal.Instance.GetDtoListFromPlaylistPersonalList(mockPlaylistPersonalList);
        mapperMock.Setup(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<IEnumerable<PlaylistPersonal>>())).Returns(playlistPersonalDtos);
        playlistPersonalRepositoryMock.Setup(repo => repo.FindAll()).Returns(mockPlaylistPersonalList);

        // Act
        var result = playlistPersonalService.FindAll();

        // Assert
        mapperMock.Verify(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<IEnumerable<PlaylistPersonal>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockPlaylistPersonalList.Count, result.Count);
    }

    [Fact]
    public void FindAllByUserId_PlaylistPersonalServices_Successfully()
    {
        // Arrange
        var playlistPersonalDtos = MockPlaylistPersonal.Instance.GetDtoListFromPlaylistPersonalList(mockPlaylistPersonalList);
        var userId = mockPlaylistPersonalList.First().Customer.User.Id;
        var customerId = mockPlaylistPersonalList.First().Customer.Id;

        mapperMock.Setup(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<List<PlaylistPersonal>>())).Returns(playlistPersonalDtos.FindAll(c => c.CustomerId.Equals(customerId)));
        playlistPersonalRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>())).Returns(mockPlaylistPersonalList);
        customerRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(new List<Customer> { mockPlaylistPersonalList.First().Customer });
        
        // Act
        var result = playlistPersonalService.FindAll(userId);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<PlaylistPersonal, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<PlaylistPersonalDto>>(It.IsAny<List<PlaylistPersonal>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockPlaylistPersonalList.FindAll(c => c.Customer.Id.Equals(customerId)).Count, result.Count);
        Assert.All(result, playlistPersonalDto => Assert.Equal(customerId, playlistPersonalDto.CustomerId));
    }

    [Fact]
    public void FindById_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = mockPlaylistPersonalList.Last();
        var playlistPersonalId = mockPlaylistPersonalService.Id;
        var playlistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPlaylistPersonalService);

        playlistPersonalRepositoryMock.Setup(repo => repo.GetById(playlistPersonalId)).Returns(mockPlaylistPersonalService);
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(playlistPersonalDto);

        // Act
        var result = playlistPersonalService.FindById(playlistPersonalId);

        // Assert
        playlistPersonalRepositoryMock.Verify(repo => repo.GetById(playlistPersonalId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonalDto>(mockPlaylistPersonalService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockPlaylistPersonalService.Name, result.Name);
    }

    [Fact]
    public void Update_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockMusics = MockMusic.Instance.GetListFaker(3);
        var mockCustomer = MockCustomer.Instance.GetListFaker(1);
        var mockPersonalPlaylist = MockPlaylistPersonal.Instance.GetFaker(mockMusics);                
        var playlistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPersonalPlaylist);
        mockPersonalPlaylist.Customer = mockCustomer.FirstOrDefault();
        mockPersonalPlaylist.CustomerId = mockCustomer.FirstOrDefault().Id;
                
        playlistPersonalRepositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(mockPersonalPlaylist);        
        customerRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(mockCustomer);
        musicRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<Music, bool>>>())).Returns(mockMusics);
        playlistPersonalRepositoryMock.Setup(repo => repo.Update(mockPersonalPlaylist));
        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>())).Returns(playlistPersonalDto);

        // Act
        var result = playlistPersonalService.Update(playlistPersonalDto);

        // Assert        
        playlistPersonalRepositoryMock.Verify(repo => repo.Update(mockPersonalPlaylist), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonalDto>(It.IsAny<PlaylistPersonal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(playlistPersonalDto.Name, result.Name);
    }

    [Fact]
    public void Delete_PlaylistPersonalService_Successfully()
    {
        // Arrange
        var mockPlaylistPersonalService = MockPlaylistPersonal.Instance.GetFaker();
        var playlistPersonalDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(mockPlaylistPersonalService);

        mapperMock.Setup(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>())).Returns(mockPlaylistPersonalService);
        playlistPersonalRepositoryMock.Setup(repo => repo.Delete(mockPlaylistPersonalService));

        // Act
        var result = playlistPersonalService.Delete(playlistPersonalDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<PlaylistPersonal>(It.IsAny<PlaylistPersonalDto>()), Times.Once);
        playlistPersonalRepositoryMock.Verify(repo => repo.Delete(mockPlaylistPersonalService), Times.Once);

        Assert.True(result);
    }
}