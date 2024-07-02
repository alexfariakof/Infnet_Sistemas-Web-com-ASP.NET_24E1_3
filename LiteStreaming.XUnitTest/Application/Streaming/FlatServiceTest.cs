using Moq;
using System.Linq.Expressions;
using Application.Streaming.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class FlatServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Flat>> flatRepositoryMock;
    private readonly FlatService flatService;
    private readonly List<Flat> mockFlatList = MockFlat.Instance.GetListFaker(3);

    public FlatServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        flatRepositoryMock = new Mock<IRepository<Flat>>();
        flatService = new FlatService(mapperMock.Object, flatRepositoryMock.Object);
    }

    [Fact]
    public void Create_FlatService_Successfully()
    {
        // Arrange
        var mockFlatService = MockFlat.Instance.GetFaker();
        var flatDto = new FlatDto
        {
            Id = mockFlatService.Id,
            Name = mockFlatService.Name,
            Description = mockFlatService.Description,
            Value = mockFlatService.Value,
        };

        flatRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Flat, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>())).Returns(mockFlatService);
        mapperMock.Setup(mapper => mapper.Map<FlatDto>(It.IsAny<Flat>())).Returns(flatDto);

        // Act
        var result = flatService.Create(flatDto);

        // Assert
        flatRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Flat, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.Save(It.IsAny<Flat>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(flatDto.Id, result.Id);
        Assert.Equal(flatDto.Name, result.Name);
        Assert.Equal(flatDto.Description, result.Description);
        Assert.Equal(flatDto.Value, result.Value);
        Assert.NotNull(result.FormattedValue);
    }

    [Fact]
    public void FindAll_FlatServices_Successfully()
    {
        // Arrange
        var flatDtos = MockFlat.Instance.GetDtoListFromFlatList(mockFlatList);
        mapperMock.Setup(mapper => mapper.Map<List<FlatDto>>(It.IsAny<List<Flat>>())).Returns(flatDtos);

        // Act
        var result = flatService.FindAll();

        // Assert
        flatRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<FlatDto>>(It.IsAny<List<Flat>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockFlatList.Count, result.Count);
    }

    [Fact]
    public void FindById_FlatService_Successfully()
    {
        // Arrange
        var mockFlatService = mockFlatList.Last();
        var flatId = mockFlatService.Id;
        var flatDto = MockFlat.Instance.GetDtoFromFlat(mockFlatService);

        flatRepositoryMock.Setup(repo => repo.GetById(flatId)).Returns(mockFlatService);
        mapperMock.Setup(mapper => mapper.Map<FlatDto>(It.IsAny<Flat>())).Returns(flatDto);

        // Act
        var result = flatService.FindById(flatId);

        // Assert
        flatRepositoryMock.Verify(repo => repo.GetById(flatId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<FlatDto>(mockFlatService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockFlatService.Name, result.Name);
    }

    [Fact]
    public void Update_FlatService_Successfully()
    {
        // Arrange
        var mockFlatService = MockFlat.Instance.GetFaker();
        var flatDto = MockFlat.Instance.GetDtoFromFlat(mockFlatService);

        mapperMock.Setup(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>())).Returns(mockFlatService);
        flatRepositoryMock.Setup(repo => repo.Update(mockFlatService));
        mapperMock.Setup(mapper => mapper.Map<FlatDto>(It.IsAny<Flat>())).Returns(flatDto);

        // Act
        var result = flatService.Update(flatDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.Update(mockFlatService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<FlatDto>(It.IsAny<Flat>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(flatDto.Name, result.Name);
    }

    [Fact]
    public void Delete_FlatService_Successfully()
    {
        // Arrange
        var mockFlatService = MockFlat.Instance.GetFaker();
        var flatDto = MockFlat.Instance.GetDtoFromFlat(mockFlatService);

        mapperMock.Setup(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>())).Returns(mockFlatService);
        flatRepositoryMock.Setup(repo => repo.Delete(mockFlatService));

        // Act
        var result = flatService.Delete(flatDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Flat>(It.IsAny<FlatDto>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.Delete(mockFlatService), Times.Once);

        Assert.True(result);
    }
}