using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Moq;
using Domain.Account.ValueObject;

namespace Repository.Persistency.Account;
public class MerchantRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public MerchantRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "TestDatabase Merchant").Options;
        contextMock = new Mock<RegisterContext>(options);    }

    [Fact]
    public void Save_Should_Add_Merchant_And_SaveChanges()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var mockMerchant = MockMerchant.Instance.GetFaker();
        mockMerchant.Cards = MockCard.Instance.GetListFaker(1);
        contextMock.Setup(c => c.Set<PerfilUser>().Find(It.IsAny<int>())).Returns(mockMerchant.User.PerfilType);

        // Act
        repository.Save(mockMerchant);

        // Assert
        contextMock.Verify(c => c.Add(mockMerchant), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Merchant_And_SaveChanges()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var mockMerchant = MockMerchant.Instance.GetFaker();
        mockMerchant.Cards = MockCard.Instance.GetListFaker(1);
        contextMock.Setup(c => c.Set<PerfilUser>().Find(It.IsAny<int>())).Returns(mockMerchant.User.PerfilType);

        // Act
        repository.Update(mockMerchant);

        // Assert
        contextMock.Verify(c => c.Update(mockMerchant), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Merchant_And_SaveChanges()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var mockMerchant = MockMerchant.Instance.GetFaker();
        mockMerchant.Cards = MockCard.Instance.GetListFaker(1);

        // Act
        repository.Delete(mockMerchant);

        // Assert
        contextMock.Verify(c => c.Remove(mockMerchant), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Merchants()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var customers = MockMerchant.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Merchant>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.FindAll();

        // Assert
        Assert.Equal(customers.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Merchant_With_Correct_Id()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var customer = MockMerchant.Instance.GetFaker();
        var customerId = customer.Id;        

        contextMock.Setup(c => c.Set<Merchant>().Find(customerId)).Returns(customer);

        // Act
        var result = repository.GetById(customerId);

        // Assert
        Assert.Equal(customer, result);
    }

    [Fact]
    public void Find_Should_Return_Merchants_Matching_Expression()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var customers = MockMerchant.Instance.GetListFaker(10);
        var mockMerchant = customers.First();
        var dbSetMock = Usings.MockDbSet(customers);        
        contextMock.Setup(c => c.Set<Merchant>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(c => c.Id == mockMerchant.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockMerchant.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Merchants_Match_Expression()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var customers = MockMerchant.Instance.GetListFaker(10);
        var mockMerchant = customers.First();

        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Merchant>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(c => c.Name == mockMerchant.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Merchants_Match_Expression()
    {
        // Arrange
        var repository = new MerchantRepository(contextMock.Object);
        var customers = MockMerchant.Instance.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Merchant>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(c => c.Name == "Steve Doe");

        // Assert
        Assert.False(result);
    }
}