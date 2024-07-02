using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;
using Moq;

namespace Repository.Persistency.Streaming;
public class BandRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public BandRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase Band").Options;
        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Band_And_SaveChanges()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var mockBand = MockBand.Instance.GetFaker();

        // Act
        repository.Save(mockBand);

        // Assert
        contextMock.Verify(c => c.Add(mockBand), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Band_And_SaveChanges()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var mockBand = MockBand.Instance.GetFaker();

        // Act
        repository.Update(mockBand);

        // Assert
        contextMock.Verify(c => c.Update(mockBand), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Band_And_SaveChanges()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var mockBand = MockBand.Instance.GetFaker();

        // Act
        repository.Delete(mockBand);

        // Assert
        contextMock.Verify(c => c.Remove(mockBand), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Bands()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var bands = MockBand.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(bands);
        contextMock.Setup(c => c.Set<Band>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(bands.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Band_With_Correct_Id()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var band = MockBand.Instance.GetFaker();
        var bandId = band.Id;

        contextMock.Setup(c => c.Set<Band>().Find(bandId)).Returns(band);

        // Act
        var result = repository.GetById(bandId);

        // Assert
        Assert.Equal(band, result);
    }

    [Fact]
    public void Find_Should_Return_Bands_Matching_Expression()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var bands = MockBand.Instance.GetListFaker(3);
        var mockBand = bands.First();
        var dbSetMock = Usings.MockDbSet(bands);
        contextMock.Setup(c => c.Set<Band>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockBand.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockBand.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Bands_Match_Expression()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var bands = MockBand.Instance.GetListFaker(10);
        var mockBand = bands.First();

        var dbSetMock = Usings.MockDbSet(bands);
        contextMock.Setup(c => c.Set<Band>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockBand.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Bands_Match_Expression()
    {
        // Arrange
        var repository = new BandRepository(contextMock.Object);
        var bands = MockBand.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(bands);
        contextMock.Setup(c => c.Set<Band>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample Band");

        // Assert
        Assert.False(result);
    }
}