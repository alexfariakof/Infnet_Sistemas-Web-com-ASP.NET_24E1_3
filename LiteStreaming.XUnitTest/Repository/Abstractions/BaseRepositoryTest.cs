using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.Persistency.Abstractions;

namespace Repository.Abstractions;
public sealed class BaseRepositoryTest
{
    private readonly Fixture fixture;
    private readonly Mock<DbContext> dbContextMock;
    private readonly Mock<DbSet<MokcEntity>> dbSetMock;
    private readonly MockBaseRepository repository;

    public class MokcEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class MockBaseRepository : BaseRepository<MokcEntity>
    {
        public MockBaseRepository(DbContext context) : base(context)  { }
    }

    public BaseRepositoryTest()
    {
        fixture = new Fixture();
        dbContextMock = new Mock<DbContext>();
        dbSetMock = new Mock<DbSet<MokcEntity>>();

        // Configuração do DbSet
        var entities = fixture.CreateMany<MokcEntity>().ToList().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        
        dbContextMock.Setup(c => c.Set<MokcEntity>()).Returns(dbSetMock.Object);

        repository = new MockBaseRepository(dbContextMock.Object);
    }

    [Fact]
    public void Save_Should_Add_Entity()
    {
        var entity = fixture.Create<MokcEntity>();

        repository.Save(entity);

        dbContextMock.Verify(c => c.Add(It.IsAny<MokcEntity>()), Times.Once);
        dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Entity()
    {
        var entity = fixture.Create<MokcEntity>();

        repository.Update(entity);

        dbContextMock.Verify(c => c.Update(It.IsAny<MokcEntity>()), Times.Once);
        dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Entity()
    {
        var entity = fixture.Create<MokcEntity>();

        repository.Delete(entity);

        dbContextMock.Verify(c => c.Remove(It.IsAny<MokcEntity>()), Times.Once);
        dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetById_Should_Return_Entity_By_Guid()
    {
        var id = Guid.NewGuid();
        var entity = fixture.Create<MokcEntity>();
        dbSetMock.Setup(s => s.Find(id)).Returns(entity);

        var result = repository.GetById(id);

        Assert.Equal(entity, result);
    }

    [Fact]
    public void GetById_Should_Return_New_Entity_If_Not_Found_By_Guid()
    {
        var id = Guid.NewGuid();
        dbSetMock.Setup(s => s.Find(id)).Returns(() => null);

        var result = repository.GetById(id);

        Assert.NotNull(result);
        Assert.IsType<MokcEntity>(result);
    }

    [Fact]
    public void GetById_Should_Return_Entity_By_Int()
    {
        var id = fixture.Create<int>();
        var entity = fixture.Create<MokcEntity>();
        dbSetMock.Setup(s => s.Find(id)).Returns(entity);

        var result = repository.GetById(id);

        Assert.Equal(entity, result);
    }

    [Fact]
    public void GetById_Should_Return_New_Entity_If_Not_Found_By_Int()
    {
        var id = fixture.Create<int>();
        dbSetMock.Setup(s => s.Find(id)).Returns(() => null);

        var result = repository.GetById(id);

        Assert.NotNull(result);
        Assert.IsType<MokcEntity>(result);
    }

    [Fact]
    public void Find_Should_Return_Entities_By_Expression()
    {
        var entities = fixture.CreateMany<MokcEntity>().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        var result = repository.Find(e => e.Name != null);

        Assert.Equal(entities, result);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Entities_Exist()
    {
        var entities = fixture.CreateMany<MokcEntity>().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        var result = repository.Exists(e => e.Name != null);

        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_Entities_Do_Not_Exist()
    {
        var entities = new List<MokcEntity>().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        var result = repository.Exists(e => e.Name != null);

        Assert.False(result);
    }

    [Fact]
    public void FindAll_Should_Return_All_Entities()
    {
        var entities = fixture.CreateMany<MokcEntity>().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        var result = repository.FindAll();

        Assert.Equal(entities, result);
    }

    [Fact]
    public void FindAllSorted_Should_Return_All_Entities_Sorted_By_Default_Property()
    {
        var entities = fixture.CreateMany<MokcEntity>().AsQueryable();
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        dbSetMock.As<IQueryable<MokcEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

        var result = repository.FindAllOrdered();

        Assert.Equivalent(entities.ToList(), result);
    }  
}