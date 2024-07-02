using Repository.Persistency.Abstractions;
using Domain.Administrative.Agreggates;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using __mock__.Admin;

namespace Repository.Abstractions;
public sealed class BaseRepositoryAdministrativeContextTest
{
    public class TestRepository : BaseRepository<AdminAccount>
    {
        public TestRepository(RegisterContextAdmin context) : base(context) { } 
    }

    private Mock<RegisterContextAdmin> contextMock;
    public BaseRepositoryAdministrativeContextTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_RegisterContextAdmin").Options;
        contextMock = new Mock<RegisterContextAdmin>(options);
    }

    [Fact]
    public void Save_Should_Add_Entity_And_SaveChanges()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entity = new AdminAccount();

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
        var entity = new AdminAccount();

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
        var entity = new AdminAccount();

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
        var mockEntities = MockAdminAccount.Instance.GetListFaker();
        var dbSetMock = Usings.MockDbSet(mockEntities);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(mockEntities.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Entity_With_Correct_Id()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var entityId = Guid.NewGuid();
        var entity = new AdminAccount { Id = entityId };

        contextMock.Setup(c => c.Set<AdminAccount>().Find(entityId)).Returns(entity);

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
        var mockEntities = MockAdminAccount.Instance.GetListFaker();
        var mockEntitie = mockEntities.First();
        var dbSetMock = Usings.MockDbSet(mockEntities);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(e => e.Name == mockEntitie.Name);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockEntitie.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Entities_Match_Expression()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var mockEntities = MockAdminAccount.Instance.GetListFaker();
        var mockEntitie = mockEntities.First();
        var dbSetMock = Usings.MockDbSet(mockEntities);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(e => e.Name == mockEntitie.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Entities_Match_Expression()
    {
        // Arrange
        var repository = new TestRepository(contextMock.Object);
        var mockEntities = MockAdminAccount.Instance.GetListFaker();
        var mockEntitie = mockEntities.Last();
        var dbSetMock = Usings.MockDbSet(mockEntities);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(e => e.Name == "mockEntitie.Name");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_Ordered_By_Ascending_DefaultProperty()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Should_Return_All_Entities_Ordered_By_Ascending_Specified_DefaultProperty").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);

            // Act
            var result = repository.FindAllOrdered();

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }      

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_Ordered_By_Ascending_Specified_Property()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Property__Ordered_By_Ascending").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);

            // Act
            var result = repository.FindAllOrdered(null, nameof(AdminAccount.Name), SortOrder.Ascending);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_Ordered_By_Desscending_Specified_Property()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Property__Ordered_By_Descending").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);

            // Act
            var result = repository.FindAllOrdered(null, nameof(AdminAccount.Name), SortOrder.Descending);

            // Assert
            var sortedEntities = mockEntities.OrderByDescending(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_Ordered_By_Ascending_Specified_Navigation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Ordered_By_Ascending").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);

            // Act
            var result = repository.FindAllOrdered(null, nameof(AdminAccount.Login.Email), SortOrder.Ascending);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Login.Email).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_Ordered_By_Descending_Specified_Navigation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Ordered_By_Descending").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);

            // Act
            var result = repository.FindAllOrdered(null, nameof(AdminAccount.Login.Email), SortOrder.Descending);

            // Assert
            var sortedEntities = mockEntities.OrderByDescending(account => account.Login.Email).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_Should_Return_All_Entities_When_SearchParams_Is_Null()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Null_Params").Options;
        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);

            string? searchParams = null;

            // Act
            var result = repository.FindAllOrdered(searchParams);

            // Assert
            Assert.Equal(mockEntities.Count, result.Count());
            Assert.Equal(mockEntities.OrderBy(account => account.Name), result);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_With_Correct_Search_Params_On_Public_Properties()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Public_Properties").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);
            string? searchParams = mockEntities.Last().Name;

            // Act
            var result = repository.FindAllOrdered(searchParams);

            // Assert
            var expectedEntities = mockEntities.Where(account => account.Name.ToLower().Contains(searchParams.ToLower())).ToList();

            Assert.Equal(expectedEntities.Count, result.Count());
            Assert.Equal(expectedEntities.First().Id, result.First().Id);
            Assert.Equal(expectedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_With_Correct_Search_Params_On_Navigation_Properties()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Navigation_Properties").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);
            string searchParams = mockEntities.Last().Login.Email;

            // Act
            var result = repository.FindAllOrdered(searchParams);

            // Assert
            var expectedEntities = mockEntities.Where(e => e.Login.Email.ToLower().Contains(searchParams.ToLower())).ToList();

            Assert.Equal(expectedEntities.Count, result.Count());
            Assert.Equal(expectedEntities.First().Id, result.First().Id);
            Assert.Equal(expectedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Ascending_Default_Property_When_SearchParams_Is_Null()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Null_Params_Ascending").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null;
            string? propertyToSort = null;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Descending_Default_Property_When_SearchParams_Is_Null()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_Null_Params_Descending").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null;
            string? propertyToSort = null;
            SortOrder sortOrder = SortOrder.Descending;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort, sortOrder);

            // Assert
            var sortedEntities = mockEntities.OrderByDescending(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Ascending_DefaultProperty()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Default_Property_Ordered_By_Ascending").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();
            var repository = new TestRepository(context);
            string? searchParams = null;

            // Act
            var result = repository.FindAllOrdered(searchParams);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Ascending_Specified_Property()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Property_Ordered_By_Ascending").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null;
            string propertyToSort = nameof(AdminAccount.Name);
            SortOrder sortOrder = SortOrder.Ascending;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort, sortOrder);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Descending_Specified_Property()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_Property_Ordered_By_Descending").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null; 
            string propertyToSort = nameof(AdminAccount.Name);
            SortOrder sortOrder = SortOrder.Descending;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort, sortOrder);

            // Assert
            var sortedEntities = mockEntities.OrderByDescending(account => account.Name).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Ascending_Specified_Navigation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Ascending_Specified_Navigation").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null;
            string propertyToSort = nameof(AdminAccount.Login.Email);
            SortOrder sortOrder = SortOrder.Ascending;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort, sortOrder);

            // Assert
            var sortedEntities = mockEntities.OrderBy(account => account.Login.Email).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }

    [Fact]
    public void FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Descending_Specified_Navigation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_FindAllOrdered_By_SearchParams_Should_Return_Entities_Ordered_By_Descending_Specified_Navigation").Options;

        using (var context = new RegisterContextAdmin(options))
        {
            var mockEntities = MockAdminAccount.Instance.GetListFaker();
            context.Set<AdminAccount>().AddRange(mockEntities);
            context.SaveChanges();

            var repository = new TestRepository(context);
            string? searchParams = null;
            string propertyToSort = nameof(AdminAccount.Login.Email);
            SortOrder sortOrder = SortOrder.Descending;

            // Act
            var result = repository.FindAllOrdered(searchParams, propertyToSort, sortOrder);

            // Assert
            var sortedEntities = mockEntities.OrderByDescending(account => account.Login.Email).ToList();
            Assert.Equal(sortedEntities.Count, result.Count());
            Assert.Equal(sortedEntities.First().Id, result.First().Id);
            Assert.Equal(sortedEntities.Last().Id, result.Last().Id);
        }
    }
}