using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Repositories;
public class MusicRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public MusicRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase Music")
            .Options;

        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Music_And_SaveChanges()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var mockMusic = MockMusic.Instance.GetFaker();

        // Act
        repository.Save(mockMusic);

        // Assert
        contextMock.Verify(c => c.Add(mockMusic), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Music_And_SaveChanges()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var mockMusic = MockMusic.Instance.GetFaker();

        // Act
        repository.Update(mockMusic);

        // Assert
        contextMock.Verify(c => c.Update(mockMusic), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Music_And_SaveChanges()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var mockMusic = MockMusic.Instance.GetFaker();

        // Act
        repository.Delete(mockMusic);

        // Assert
        contextMock.Verify(c => c.Remove(mockMusic), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Musics()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var musics = MockMusic.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(musics);
        contextMock.Setup(c => c.Set<Music>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(musics.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Music_With_Correct_Id()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var music = MockMusic.Instance.GetFaker();
        var musicId = music.Id;

        contextMock.Setup(c => c.Set<Music>().Find(musicId)).Returns(music);

        // Act
        var result = repository.GetById(musicId);

        // Assert
        Assert.Equal(music, result);
    }

    [Fact]
    public void Find_Should_Return_Musics_Matching_Expression()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var musics = MockMusic.Instance.GetListFaker(3);
        var mockMusic = musics.First();
        var dbSetMock = Usings.MockDbSet(musics);
        contextMock.Setup(c => c.Set<Music>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockMusic.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockMusic.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Musics_Match_Expression()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var musics = MockMusic.Instance.GetListFaker(10);
        var mockMusic = musics.First();

        var dbSetMock = Usings.MockDbSet(musics);
        contextMock.Setup(c => c.Set<Music>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockMusic.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Musics_Match_Expression()
    {
        // Arrange
        var repository = new MusicRepository(contextMock.Object);
        var musics = MockMusic.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(musics);
        contextMock.Setup(c => c.Set<Music>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Music");

        // Assert
        Assert.False(result);
    }
}