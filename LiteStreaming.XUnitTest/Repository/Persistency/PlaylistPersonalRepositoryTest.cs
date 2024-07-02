using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Repository.Persistency.Account;
public class PlaylistPersonalRepositoryTest
{
    private Mock<RegisterContext> contextMock;
    public PlaylistPersonalRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase PlaylistPersonal").Options;
        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_PlaylistPersonal_And_SaveChanges()
    {
        // Arrange
        var mockMusics = MockMusic.Instance.GetListFaker(1);
        var mockPlaylistPersonal = MockPlaylistPersonal.Instance.GetListFaker(1, mockMusics);
        var mockCustomer = mockPlaylistPersonal.Select(p => p.Customer).ToList();
        contextMock.Object.Add(mockMusics);
        contextMock.Setup(dsMusics => dsMusics.Set<Music>()).Returns(Usings.MockDbSet<Music>(mockMusics).Object);
        contextMock.Setup(dsCustomer => dsCustomer.Set<Customer?>()).Returns(Usings.MockDbSet<Customer?>(mockCustomer).Object);
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var playlistPersonal = MockPlaylistPersonal.Instance.GetFaker();

        // Act
        repository.Save(playlistPersonal);

        // Assert
        contextMock.Verify(c => c.Add(It.IsAny<PlaylistPersonal>()), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_PlaylistPersonal_And_SaveChanges()
    {
        // Arrange
        var mockMusics = MockMusic.Instance.GetListFaker(1);
        var mockPlaylistPersonal = MockPlaylistPersonal.Instance.GetListFaker(2, mockMusics);
        var mockCustomer = mockPlaylistPersonal.Select(p => p.Customer).ToList();
        contextMock.Object.Add(mockPlaylistPersonal);
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        contextMock.Setup(dsPersonalPlaylist => dsPersonalPlaylist.Set<PlaylistPersonal>()).Returns(Usings.MockDbSet<PlaylistPersonal>(mockPlaylistPersonal).Object);
        contextMock.Setup(dsMusics => dsMusics.Set<Music>()).Returns(Usings.MockDbSet<Music>(mockMusics).Object);
        contextMock.Setup(dsCustomer => dsCustomer.Set<Customer?>()).Returns(Usings.MockDbSet<Customer?>(mockCustomer).Object);

        var playlistPersonal = mockPlaylistPersonal.First();
        playlistPersonal.Musics.First().Name = "Teste Repository Update";

        // Act
        repository.Update(playlistPersonal);

        // Assert
        contextMock.Verify(c => c.Update(It.IsAny<PlaylistPersonal>()), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_PlaylistPersonal_And_SaveChanges()
    {
        // Arrange
        var mockPlaylistPersonal = MockPlaylistPersonal.Instance.GetListFaker(5, MockMusic.Instance.GetListFaker(2));
        contextMock.Object.Add(mockPlaylistPersonal);
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        // Act
        repository.Delete(mockPlaylistPersonal.First());

        // Assert
        contextMock.Verify(c => c.Remove(It.IsAny<PlaylistPersonal>()), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_PlaylistPersonals()
    {
        // Arrange
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var PlaylistPersonals = MockPlaylistPersonal.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(PlaylistPersonals);
        contextMock.Setup(c => c.Set<PlaylistPersonal>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(PlaylistPersonals.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_PlaylistPersonal_With_Correct_Id()
    {
        // Arrange
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var PlaylistPersonal = MockPlaylistPersonal.Instance.GetFaker();
        var PlaylistPersonalId = PlaylistPersonal.Id;

        contextMock.Setup(c => c.Set<PlaylistPersonal>().Find(PlaylistPersonalId)).Returns(PlaylistPersonal);

        // Act
        var result = repository.GetById(PlaylistPersonalId);

        // Assert
        Assert.Equal(PlaylistPersonal, result);
    }

    [Fact]
    public void Find_Should_Return_PlaylistPersonals_Matching_Expression()
    {
        // Arrange
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var PlaylistPersonals = MockPlaylistPersonal.Instance.GetListFaker(3);
        var mockPlaylistPersonal = PlaylistPersonals.First();
        var dbSetMock = Usings.MockDbSet(PlaylistPersonals);
        contextMock.Setup(c => c.Set<PlaylistPersonal>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockPlaylistPersonal.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockPlaylistPersonal.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_PlaylistPersonals_Match_Expression()
    {
        // Arrange
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var PlaylistPersonals = MockPlaylistPersonal.Instance.GetListFaker(10);
        var mockPlaylistPersonal = PlaylistPersonals.First();

        var dbSetMock = Usings.MockDbSet(PlaylistPersonals);
        contextMock.Setup(c => c.Set<PlaylistPersonal>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockPlaylistPersonal.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_PlaylistPersonals_Match_Expression()
    {
        // Arrange
        var repository = new PlaylistPersonalRepository(contextMock.Object);
        var PlaylistPersonals = MockPlaylistPersonal.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(PlaylistPersonals);
        contextMock.Setup(c => c.Set<PlaylistPersonal>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample PlaylistPersonal");

        // Assert
        Assert.False(result);
    }
}