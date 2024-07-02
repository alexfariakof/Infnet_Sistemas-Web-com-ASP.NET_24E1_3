using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Persistency.Streaming;
public class PlaylistRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public PlaylistRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase Playlist").Options;
        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Playlist_And_SaveChanges()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var mockPlaylist = MockPlaylist.Instance.GetFaker();

        // Act
        repository.Save(mockPlaylist);

        // Assert
        contextMock.Verify(c => c.Add(mockPlaylist), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Playlist_And_SaveChanges()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var mockPlaylist = MockPlaylist.Instance.GetFaker();

        // Act
        repository.Update(mockPlaylist);

        // Assert
        contextMock.Verify(c => c.Update(mockPlaylist), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Playlist_And_SaveChanges()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var mockPlaylist = MockPlaylist.Instance.GetFaker();

        // Act
        repository.Delete(mockPlaylist);

        // Assert
        contextMock.Verify(c => c.Remove(mockPlaylist), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Playlists()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var playlists = MockPlaylist.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(playlists);
        contextMock.Setup(c => c.Set<Playlist>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(playlists.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Playlist_With_Correct_Id()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var playlist = MockPlaylist.Instance.GetFaker();
        var playlistId = playlist.Id;

        contextMock.Setup(c => c.Set<Playlist>().Find(playlistId)).Returns(playlist);

        // Act
        var result = repository.GetById(playlistId);

        // Assert
        Assert.Equal(playlist, result);
    }

    [Fact]
    public void Find_Should_Return_Playlists_Matching_Expression()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var playlists = MockPlaylist.Instance.GetListFaker(3);
        var mockPlaylist = playlists.First();
        var dbSetMock = Usings.MockDbSet(playlists);
        contextMock.Setup(c => c.Set<Playlist>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockPlaylist.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockPlaylist.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Playlists_Match_Expression()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var playlists = MockPlaylist.Instance.GetListFaker(10);
        var mockPlaylist = playlists.First();

        var dbSetMock = Usings.MockDbSet(playlists);
        contextMock.Setup(c => c.Set<Playlist>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockPlaylist.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Playlists_Match_Expression()
    {
        // Arrange
        var repository = new PlaylistRepository(contextMock.Object);
        var playlists = MockPlaylist.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(playlists);
        contextMock.Setup(c => c.Set<Playlist>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Playlist");

        // Assert
        Assert.False(result);
    }
}