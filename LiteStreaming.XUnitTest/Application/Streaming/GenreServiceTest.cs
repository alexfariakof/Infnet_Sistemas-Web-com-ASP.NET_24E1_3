using Moq;
using System.Linq.Expressions;
using Application.Streaming.Dto;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class GenreServiceTest
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepository<Genre>> genreRepositoryMock;
    private readonly GenreService genreService;
    private readonly List<Genre> mockGenreList = MockGenre.Instance.GetListFaker(3);

    public GenreServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        genreRepositoryMock = new Mock<IRepository<Genre>>();
        genreService = new GenreService(mapperMock.Object, genreRepositoryMock.Object);
    }

    [Fact]
    public void Create_GenreService_Successfully()
    {
        // Arrange
        var mockGenreService = MockGenre.Instance.GetFaker();
        var genreDto = new GenreDto
        {
            Id = mockGenreService.Id,
            Name = mockGenreService.Name,
        };

        genreRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Genre, bool>>>())).Returns(false);
        mapperMock.Setup(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>())).Returns(mockGenreService);
        mapperMock.Setup(mapper => mapper.Map<GenreDto>(It.IsAny<Genre>())).Returns(genreDto);

        // Act
        var result = genreService.Create(genreDto);

        // Assert
        genreRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>()), Times.Once);
        genreRepositoryMock.Verify(repo => repo.Save(It.IsAny<Genre>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(genreDto.Id, result.Id);
        Assert.Equal(genreDto.Name, result.Name);
    }

    [Fact]
    public void FindAll_GenreServices_Successfully()
    {
        // Arrange
        var genreDtos = MockGenre.Instance.GetDtoListFromGenreList(mockGenreList);
        mapperMock.Setup(mapper => mapper.Map<List<GenreDto>>(It.IsAny<List<Genre>>())).Returns(genreDtos);

        // Act
        var result = genreService.FindAll();

        // Assert
        genreRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<GenreDto>>(It.IsAny<List<Genre>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockGenreList.Count, result.Count);
    }

    [Fact]
    public void FindById_GenreService_Successfully()
    {
        // Arrange
        var mockGenreService = mockGenreList.Last();
        var genreId = mockGenreService.Id;
        var genreDto = MockGenre.Instance.GetDtoFromGenre(mockGenreService);

        genreRepositoryMock.Setup(repo => repo.GetById(genreId)).Returns(mockGenreService);
        mapperMock.Setup(mapper => mapper.Map<GenreDto>(It.IsAny<Genre>())).Returns(genreDto);

        // Act
        var result = genreService.FindById(genreId);

        // Assert
        genreRepositoryMock.Verify(repo => repo.GetById(genreId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<GenreDto>(mockGenreService), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockGenreService.Name, result.Name);
    }

    [Fact]
    public void Update_GenreService_Successfully()
    {
        // Arrange
        var mockGenreService = MockGenre.Instance.GetFaker();
        var genreDto = MockGenre.Instance.GetDtoFromGenre(mockGenreService);

        mapperMock.Setup(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>())).Returns(mockGenreService);
        genreRepositoryMock.Setup(repo => repo.Update(mockGenreService));
        mapperMock.Setup(mapper => mapper.Map<GenreDto>(It.IsAny<Genre>())).Returns(genreDto);

        // Act
        var result = genreService.Update(genreDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>()), Times.Once);
        genreRepositoryMock.Verify(repo => repo.Update(mockGenreService), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<GenreDto>(It.IsAny<Genre>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(genreDto.Name, result.Name);
    }

    [Fact]
    public void Delete_GenreService_Successfully()
    {
        // Arrange
        var mockGenreService = MockGenre.Instance.GetFaker();
        var genreDto = MockGenre.Instance.GetDtoFromGenre(mockGenreService);

        mapperMock.Setup(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>())).Returns(mockGenreService);
        genreRepositoryMock.Setup(repo => repo.Delete(mockGenreService));

        // Act
        var result = genreService.Delete(genreDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Genre>(It.IsAny<GenreDto>()), Times.Once);
        genreRepositoryMock.Verify(repo => repo.Delete(mockGenreService), Times.Once);

        Assert.True(result);
    }
}