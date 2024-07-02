using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Repository.Persistency.Account;
public class UserRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public UserRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase_UserRepositoryTest").Options;
        contextMock = new Mock<RegisterContext>(options);
    }

    [Fact]
    public void Save_Should_Add_User_And_SaveChanges()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var mockUser = MockUser.Instance.GetFaker();
        contextMock.Setup(c => c.Set<PerfilUser>().Find(It.IsAny<int>())).Returns(mockUser.PerfilType);

        // Act
        repository.Save(mockUser);

        // Assert
        contextMock.Verify(c => c.Add(mockUser), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_User_And_SaveChanges()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var mockUser = MockUser.Instance.GetFaker();
        contextMock.Setup(c => c.Set<PerfilUser>().Find(It.IsAny<int>())).Returns(mockUser.PerfilType);

        // Act
        repository.Update(mockUser);

        // Assert
        contextMock.Verify(c => c.Update(mockUser), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_User_And_SaveChanges()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var mockUser = MockUser.Instance.GetFaker();

        // Act
        repository.Delete(mockUser);

        // Assert
        contextMock.Verify(c => c.Remove(mockUser), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Users()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var users = MockUser.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(users);
        contextMock.Setup(c => c.Set<User>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(users.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_User_With_Correct_Id()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var user = MockUser.Instance.GetFaker();
        var userId = user.Id;

        contextMock.Setup(c => c.Set<User>().Find(userId)).Returns(user);

        // Act
        var result = repository.GetById(userId);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public void Find_Should_Return_Users_Matching_Expression()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var users = MockUser.Instance.GetListFaker(3);
        var mockUser = users.First();
        var dbSetMock = Usings.MockDbSet(users);
        contextMock.Setup(c => c.Set<User>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(f => f.Id == mockUser.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockUser.Login, result.First().Login);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Users_Match_Expression()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var users = MockUser.Instance.GetListFaker(10);
        var mockUser = users.First();

        var dbSetMock = Usings.MockDbSet(users);
        contextMock.Setup(c => c.Set<User>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Login == mockUser.Login);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Users_Match_Expression()
    {
        // Arrange
        var repository = new UserRepository(contextMock.Object);
        var users = MockUser.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(users);
        contextMock.Setup(c => c.Set<User>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(f => f.Login.Email == "teste@email.com");

        // Assert
        Assert.False(result);
    }
}