using Moq;
using System.Linq.Expressions;
using Application.Streaming.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class BandServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Band>> bandRepositoryMock;
    private readonly BandService bandService;
    private readonly List<Band> mockBandList = MockBand.Instance.GetListFaker(3);

    public BandServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        bandRepositoryMock = new Mock<IRepository<Band>>();
        bandService = new BandService(mapperMock.Object, bandRepositoryMock.Object);
    }

    [Fact]
    public void Create_BandService_Successfully()
    {
        // Arrange
        var mockBandService = MockBand.Instance.GetFaker();
        var bandDto = new BandDto
        {
            Id = mockBandService.Id,
            Name = mockBandService.Name,
            Description = mockBandService.Description,
            Backdrop = mockBandService.Backdrop,
            Albums = MockAlbum.Instance.GetDtoListFromAlbumList(mockBandService.Albums)
        };

        bandRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Band, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Band>(It.IsAny<BandDto>())).Returns(mockBandService);
        mapperMock.Setup(mapper => mapper.Map<BandDto>(It.IsAny<Band>())).Returns(bandDto);

        // Act
        var result = bandService.Create(bandDto);

        // Assert
        bandRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Band, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Band>(It.IsAny<BandDto>()), Times.Once);
        bandRepositoryMock.Verify(repo => repo.Save(It.IsAny<Band>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(bandDto.Id, result.Id);
        Assert.Equal(bandDto.Name, result.Name);
        Assert.Equal(bandDto.Description, result.Description);
        Assert.Equal(bandDto.Backdrop, result.Backdrop);
        Assert.NotNull(result.Albums);
    }

    [Fact]
    public void FindAll_BandServices_Successfully()
    {
        // Arrange
        var bandDtos = MockBand.Instance.GetDtoListFromBandList(mockBandList);
        mapperMock.Setup(mapper => mapper.Map<List<BandDto>>(It.IsAny<List<Band>>())).Returns(bandDtos);

        // Act
        var result = bandService.FindAll();

        // Assert
        bandRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<BandDto>>(It.IsAny<List<Band>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockBandList.Count, result.Count);
    }

    [Fact]
    public void FindById_BandService_Successfully()
    {
        // Arrange
        var mockBandService = mockBandList.Last();
        var bandId = mockBandService.Id;
        var bandDto = MockBand.Instance.GetDtoFromBand(mockBandService);

        bandRepositoryMock.Setup(repo => repo.GetById(bandId)).Returns(mockBandService);
        mapperMock.Setup(mapper => mapper.Map<BandDto>(It.IsAny<Band>())).Returns(bandDto);

        // Act
        var result = bandService.FindById(bandId);

        // Assert
        bandRepositoryMock.Verify(repo => repo.GetById(bandId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<BandDto>(mockBandService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockBandService.Name, result.Name);
    }

    [Fact]
    public void Update_BandService_Successfully()
    {
        // Arrange
        var mockBandService = MockBand.Instance.GetFaker();
        var bandDto = MockBand.Instance.GetDtoFromBand(mockBandService);

        mapperMock.Setup(mapper => mapper.Map<Band>(It.IsAny<BandDto>())).Returns(mockBandService);
        bandRepositoryMock.Setup(repo => repo.Update(mockBandService));
        mapperMock.Setup(mapper => mapper.Map<BandDto>(It.IsAny<Band>())).Returns(bandDto);

        // Act
        var result = bandService.Update(bandDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Band>(It.IsAny<BandDto>()), Times.Once);
        bandRepositoryMock.Verify(repo => repo.Update(mockBandService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<BandDto>(It.IsAny<Band>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(bandDto.Name, result.Name);
    }

    [Fact]
    public void Delete_BandService_Successfully()
    {
        // Arrange
        var mockBandService = MockBand.Instance.GetFaker();
        var bandDto = MockBand.Instance.GetDtoFromBand(mockBandService);

        mapperMock.Setup(mapper => mapper.Map<Band>(It.IsAny<BandDto>())).Returns(mockBandService);
        bandRepositoryMock.Setup(repo => repo.Delete(mockBandService));

        // Act
        var result = bandService.Delete(bandDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Band>(It.IsAny<BandDto>()), Times.Once);
        bandRepositoryMock.Verify(repo => repo.Delete(mockBandService), Times.Once);

        Assert.True(result);
    }
}