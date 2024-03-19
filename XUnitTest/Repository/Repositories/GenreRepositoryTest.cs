using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Repositories;
public class GenreRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public GenreRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_GenreRepositoryTest")
            .Options;

        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Genre_And_SaveChanges()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var mockGenre = MockGenre.Instance.GetFaker();

        // Act
        repository.Save(mockGenre);

        // Assert
        contextMock.Verify(c => c.Add(mockGenre), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Genre_And_SaveChanges()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var mockGenre = MockGenre.Instance.GetFaker();

        // Act
        repository.Update(mockGenre);

        // Assert
        contextMock.Verify(c => c.Update(mockGenre), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Genre_And_SaveChanges()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var mockGenre = MockGenre.Instance.GetFaker();

        // Act
        repository.Delete(mockGenre);

        // Assert
        contextMock.Verify(c => c.Remove(mockGenre), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Genres()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var genres = MockGenre.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(genres);
        contextMock.Setup(c => c.Set<Genre>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(genres.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Genre_With_Correct_Id()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var genre = MockGenre.Instance.GetFaker();
        var genreId = genre.Id;

        contextMock.Setup(c => c.Set<Genre>().Find(genreId)).Returns(genre);

        // Act
        var result = repository.GetById(genreId);

        // Assert
        Assert.Equal(genre, result);
    }

    [Fact]
    public void Find_Should_Return_Genres_Matching_Expression()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var genres = MockGenre.Instance.GetListFaker(3);
        var mockGenre = genres.First();
        var dbSetMock = Usings.MockDbSet(genres);
        contextMock.Setup(c => c.Set<Genre>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockGenre.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockGenre.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Genres_Match_Expression()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var genres = MockGenre.Instance.GetListFaker(10);
        var mockGenre = genres.First();

        var dbSetMock = Usings.MockDbSet(genres);
        contextMock.Setup(c => c.Set<Genre>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockGenre.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Genres_Match_Expression()
    {
        // Arrange
        var repository = new GenreRepository(contextMock.Object);
        var genres = MockGenre.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(genres);
        contextMock.Setup(c => c.Set<Genre>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Genre");

        // Assert
        Assert.False(result);
    }
}