﻿using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Domain.Transactions.ValueObject;
using Moq;

namespace Repository.Repositories;
public class CustomerRepositoryTest
{
    private Mock<RegisterContext> contextMock;

    public CustomerRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase Customer")
            .Options;

        contextMock = new Mock<RegisterContext>(options);
        contextMock.Setup(c => c.Set<CreditCardBrand>()).Returns(Usings.MockDataSetCreditCardBrand().Object);
    }

    [Fact]
    public void Save_Should_Add_Customer_And_SaveChanges()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var mockCustomer = MockCustomer.GetFaker();
        mockCustomer.Cards = MockCard.GetListFaker(1);
        
        // Act
        repository.Save(mockCustomer);

        // Assert
        contextMock.Verify(c => c.Add(mockCustomer), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_Should_Update_Customer_And_SaveChanges()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var mockCustomer = MockCustomer.GetFaker();
        mockCustomer.Cards = MockCard.GetListFaker(1);

        // Act
        repository.Update(mockCustomer);

        // Assert
        contextMock.Verify(c => c.Update(mockCustomer), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Should_Remove_Customer_And_SaveChanges()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var mockCustomer = MockCustomer.GetFaker();
        mockCustomer.Cards = MockCard.GetListFaker(1);

        // Act
        repository.Delete(mockCustomer);

        // Assert
        contextMock.Verify(c => c.Remove(mockCustomer), Times.Once);
        contextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAll_Should_Return_All_Customers()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var customers = MockCustomer.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Customer>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Equal(customers.Count, result.Count());
    }

    [Fact]
    public void GetById_Should_Return_Customer_With_Correct_Id()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var customer = MockCustomer.GetFaker();
        var customerId = customer.Id;        

        contextMock.Setup(c => c.Set<Customer>().Find(customerId)).Returns(customer);

        // Act
        var result = repository.GetById(customerId);

        // Assert
        Assert.Equal(customer, result);
    }

    [Fact]
    public void Find_Should_Return_Customers_Matching_Expression()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var customers = MockCustomer.GetListFaker(10);
        var mockCustomer = customers.First();
        var dbSetMock = Usings.MockDbSet(customers);        
        contextMock.Setup(c => c.Set<Customer>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Find(c => c.Name == mockCustomer.Name);

        // Assert
        Assert.Single(result);
        Assert.Equal(mockCustomer.Name, result.First().Name);
    }

    [Fact]
    public void Exists_Should_Return_True_If_Customers_Match_Expression()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var customers = MockCustomer.GetListFaker(10);
        var mockCustomer = customers.First();

        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Customer>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(c => c.Name == mockCustomer.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_Should_Return_False_If_No_Customers_Match_Expression()
    {
        // Arrange
        var repository = new CustomerRepository(contextMock.Object);
        var customers = MockCustomer.GetListFaker(10);
        var dbSetMock = Usings.MockDbSet(customers);
        contextMock.Setup(c => c.Set<Customer>()).Returns(dbSetMock.Object);

        // Act
        var result = repository.Exists(c => c.Name == "Steve Doe");

        // Assert
        Assert.False(result);
    }
}