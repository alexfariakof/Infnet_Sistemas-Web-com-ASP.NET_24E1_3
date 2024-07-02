using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Persistency.Streaming;
public class AlbumRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public AlbumRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase_AlbumRepositoryTest").Options;
        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Album_And_SaveChanges()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var mockAlbum = MockAlbum.Instance.GetFaker();

        // Act
        repository.Save(mockAlbum);

        // Assert
        contextMock.Verify(c => c.Add(mockAlbum), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Album_And_SaveChanges()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var mockAlbum = MockAlbum.Instance.GetFaker();

        // Act
        repository.Update(mockAlbum);

        // Assert
        contextMock.Verify(c => c.Update(mockAlbum), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Album_And_SaveChanges()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var mockAlbum = MockAlbum.Instance.GetFaker();

        // Act
        repository.Delete(mockAlbum);

        // Assert
        contextMock.Verify(c => c.Remove(mockAlbum), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Albums()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var albums = MockAlbum.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(albums);
        contextMock.Setup(c => c.Set<Album>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(albums.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Album_With_Correct_Id()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var album = MockAlbum.Instance.GetFaker();
        var albumId = album.Id;

        contextMock.Setup(c => c.Set<Album>().Find(albumId)).Returns(album);

        // Act
        var result = repository.GetById(albumId);

        // Assert
        Assert.Equal(album, result);
    }

    [Fact]
    public void Find_Should_Return_Albums_Matching_Expression()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var albums = MockAlbum.Instance.GetListFaker(3);
        var mockAlbum = albums.First();
        var dbSetMock = Usings.MockDbSet(albums);
        contextMock.Setup(c => c.Set<Album>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockAlbum.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockAlbum.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Albums_Match_Expression()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var albums = MockAlbum.Instance.GetListFaker(10);
        var mockAlbum = albums.First();

        var dbSetMock = Usings.MockDbSet(albums);
        contextMock.Setup(c => c.Set<Album>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockAlbum.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Albums_Match_Expression()
    {
        // Arrange
        var repository = new AlbumRepository(contextMock.Object);
        var albums = MockAlbum.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(albums);
        contextMock.Setup(c => c.Set<Album>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Album");

        // Assert
        Assert.False(result);
    }
}