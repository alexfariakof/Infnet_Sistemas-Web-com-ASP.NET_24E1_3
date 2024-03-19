using Microsoft.EntityFrameworkCore;
using Moq;

namespace Repository;
public class TestEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class TestRepository : RepositoryBase<TestEntity>
{
    public TestRepository(RegisterContext context) : base(context) { }
}
public class RepositoryBaseTest
{

    private Mock<RegisterContext> contextMock;
    public RepositoryBaseTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_Entity_And_SaveChanges()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entity = new TestEntity();

        // Act
        repository.Save(entity);

        // Assert
        contextMock.Verify(c => c.Add(entity), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Entity_And_SaveChanges()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entity = new TestEntity();

        // Act
        repository.Update(entity);

        // Assert
        contextMock.Verify(c => c.Update(entity), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Entity_And_SaveChanges()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entity = new TestEntity();

        // Act
        repository.Delete(entity);

        // Assert
        contextMock.Verify(c => c.Remove(entity), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Entities()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };
        var dbSetMock = Usings.MockDbSet(entities);
        contextMock.Setup(c => c.Set<TestEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(entities.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Entity_With_Correct_Id()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entityId = Guid.NewGuid();
        var entity = new TestEntity { Id = entityId };

        contextMock.Setup(c => c.Set<TestEntity>().Find(entityId)).Returns(entity);

        // Act
        var result = repository.GetById(entityId);

        // Assert
        Assert.Equal(entity, result);
    }

    [Fact]
    public void Find_Should_Return_Entities_Matching_Expression()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entities = new List<TestEntity> { new TestEntity { Name = "Entity1" }, new TestEntity { Name = "Entity2" } };
        var dbSetMock = Usings.MockDbSet(entities);
        contextMock.Setup(c => c.Set<TestEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(e => e.Name == "Entity1");

        // Assert
        Assert.Single(result);
        Assert.Equal("Entity1", result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Entities_Match_Expression()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entities = new List<TestEntity> { new TestEntity { Name = "Entity1" }, new TestEntity { Name = "Entity2" } };
        var dbSetMock = Usings.MockDbSet(entities);
        contextMock.Setup(c => c.Set<TestEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(e => e.Name == "Entity1");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Entities_Match_Expression()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entities = new List<TestEntity> { new TestEntity { Name = "Entity1" }, new TestEntity { Name = "Entity2" } };
        var dbSetMock = Usings.MockDbSet(entities);
        contextMock.Setup(c => c.Set<TestEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(e => e.Name == "Entity3");

        // Assert
        Assert.False(result);
    }

}
