using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Repositories;
public class FlatRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public FlatRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase Flat")
            .Options;

        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Flat_And_SaveChanges()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var mockFlat = MockFlat.Instance.GetFaker();

        // Act
        repository.Save(mockFlat);

        // Assert
        contextMock.Verify(c => c.Add(mockFlat), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Flat_And_SaveChanges()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var mockFlat = MockFlat.Instance.GetFaker();

        // Act
        repository.Update(mockFlat);

        // Assert
        contextMock.Verify(c => c.Update(mockFlat), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Flat_And_SaveChanges()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var mockFlat = MockFlat.Instance.GetFaker();

        // Act
        repository.Delete(mockFlat);

        // Assert
        contextMock.Verify(c => c.Remove(mockFlat), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Flats()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var flats = MockFlat.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(flats);
        contextMock.Setup(c => c.Set<Flat>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(flats.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Flat_With_Correct_Id()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var flat = MockFlat.Instance.GetFaker();
        var flatId = flat.Id;

        contextMock.Setup(c => c.Set<Flat>().Find(flatId)).Returns(flat);

        // Act
        var result = repository.GetById(flatId);

        // Assert
        Assert.Equal(flat, result);
    }

    [Fact]
    public void Find_Should_Return_Flats_Matching_Expression()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var flats = MockFlat.Instance.GetListFaker(3);
        var mockFlat = flats.First();
        var dbSetMock = Usings.MockDbSet(flats);
        contextMock.Setup(c => c.Set<Flat>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockFlat.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockFlat.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Flats_Match_Expression()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var flats = MockFlat.Instance.GetListFaker(10);
        var mockFlat = flats.First();

        var dbSetMock = Usings.MockDbSet(flats);
        contextMock.Setup(c => c.Set<Flat>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockFlat.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Flats_Match_Expression()
    {
        // Arrange
        var repository = new FlatRepository(contextMock.Object);
        var flats = MockFlat.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(flats);
        contextMock.Setup(c => c.Set<Flat>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Flat");

        // Assert
        Assert.False(result);
    }
}