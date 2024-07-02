using Microsoft.EntityFrameworkCore;
using Moq;
using __mock__.Admin;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;

namespace Repository.Persistency.Administrative;
public class AdminAccountRepositoryTest
{
    private Mock<RegisterContextAdmin> contextMock;

    public AdminAccountRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_AdminAccountRepositoryTest").Options;
        contextMock = new Mock<RegisterContextAdmin>(options);
    }

    [Fact]
    public void Save_Should_Add_Album_And_SaveChanges()
    {
        // Arrange
        var mockAdmin = MockAdminAccount.Instance.GetFaker();
        contextMock.Setup(c => c.Set<Perfil>().Find(It.IsAny<int>())).Returns(mockAdmin.PerfilType);
        var repository = new AdminAccountRepository(contextMock.Object);
        
        

        // Act
        repository.Save(mockAdmin);

        // Assert
        contextMock.Verify(c => c.Add(mockAdmin), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Album_And_SaveChanges()
    {
        // Arrange        
        var mockAdmin = MockAdminAccount.Instance.GetFaker();
        contextMock.Setup(c => c.Set<Perfil>().Find(It.IsAny<int>())).Returns(mockAdmin.PerfilType);
        var repository = new AdminAccountRepository(contextMock.Object);

        // Act
        repository.Update(mockAdmin);

        // Assert
        contextMock.Verify(c => c.Update(mockAdmin), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Album_And_SaveChanges()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var mockAdmin = MockAdminAccount.Instance.GetFaker();

        // Act
        repository.Delete(mockAdmin);

        // Assert
        contextMock.Verify(c => c.Remove(mockAdmin), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Albums()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var adminAccount = MockAdminAccount.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(adminAccount);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(adminAccount.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Album_With_Correct_Id()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var album = MockAdminAccount.Instance.GetFaker();
        var albumId = album.Id;

        contextMock.Setup(c => c.Set<AdminAccount>().Find(albumId)).Returns(album);

        // Act
        var result = repository.GetById(albumId);

        // Assert
        Assert.Equal(album, result);
    }

    [Fact]
    public void Find_Should_Return_Albums_Matching_Expression()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var adminAccount = MockAdminAccount.Instance.GetListFaker(3);
        var mockAdmin = adminAccount.First();
        var dbSetMock = Usings.MockDbSet(adminAccount);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockAdmin.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockAdmin.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Albums_Match_Expression()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var adminAccount = MockAdminAccount.Instance.GetListFaker(10);
        var mockAdmin = adminAccount.First();

        var dbSetMock = Usings.MockDbSet(adminAccount);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == mockAdmin.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Albums_Match_Expression()
    {
        // Arrange
        var repository = new AdminAccountRepository(contextMock.Object);
        var adminAccount = MockAdminAccount.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(adminAccount);
        contextMock.Setup(c => c.Set<AdminAccount>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Name == "Sample AdministrativeAccount");

        // Assert
        Assert.False(result);
    }
}