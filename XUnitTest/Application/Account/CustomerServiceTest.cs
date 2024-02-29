﻿using Application.Account.Dto;
using Application.Transactions.Dto;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Moq;
using System.Linq.Expressions;

namespace Application.Account;
public class CustomerServiceTest
{
    [Fact]
    public void Create_Customer_Successfully()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);
        var mockCustomer = MockCustomer.GetFaker();
        var mockCard = MockCard.GetFaker();
        var mockFlat = MockFlat.GetFaker();
        var customerDto = new CustomerDto()
        {
            Name = mockCustomer.Name,
            Email = mockCustomer.Login.Email,
            Password = mockCustomer.Login.Password,
            CPF = mockCustomer.CPF,
            Birth = mockCustomer.Birth,
            Phone = mockCustomer.Phone.Number,
            Address = new AddressDto
            {
                Zipcode = mockCustomer.Addresses.Last().Zipcode,
                Street = mockCustomer.Addresses.Last().Street,
                Number = mockCustomer.Addresses.Last().Number,
                Neighborhood = mockCustomer.Addresses.Last().Neighborhood,
                City = mockCustomer.Addresses.Last().City,
                State = mockCustomer.Addresses.Last().State,
                Complement = mockCustomer.Addresses.Last().Complement,
                Country = mockCustomer.Addresses.Last().Country
            },
            FlatId = mockFlat.Id,
            Card = new CardDto
            {
                Limit = 1000,
                Number = mockCard.Number,
                Validate = mockCard.Validate.Value,
                CVV = mockCard.CVV
            }
        };

        flatRepositoryMock.Setup(repo => repo.GetById(mockFlat.Id)).Returns(mockFlat);
        mapperMock.Setup(mapper => mapper.Map<Card>(customerDto.Card)).Returns(mockCard);
        mapperMock.Setup(mapper => mapper.Map<Address>(customerDto.Address)).Returns(mockCustomer.Addresses.Last());
        mapperMock.Setup(mapper => mapper.Map<Customer>(customerDto)).Returns(mockCustomer);
        mapperMock.Setup(mapper => mapper.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);
        

        // Act
        var result = customerService.Create(customerDto);

        // Assert
        customerRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.GetById(mockFlat.Id), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Card>(customerDto.Card), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Address>(customerDto.Address), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<CustomerDto>(It.IsAny<Customer>()), Times.Once);
        customerRepositoryMock.Verify(repo => repo.Save(It.IsAny<Customer>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(customerDto.Name, result.Name);
        Assert.Equal(customerDto.Email, result.Email);
    }

    [Fact]
    public void Create_Customer_With_Existing_Email_Fails()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);

        var customerDto = new CustomerDto {  Email = "existing.email@example.com" };

        customerRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(true);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => customerService.Create(customerDto));
        Assert.Equal("Usuário já existente na base.", exception.Message);
        customerRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Create_Customer_With_Nonexistent_Flat_Fails()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);

        var customerDto = new CustomerDto();

        flatRepositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns((Flat)null);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => customerService.Create(customerDto));
        Assert.Equal("Plano não existente ou não encontrado.", exception.Message);

        flatRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Once);
        customerRepositoryMock.Verify(repo => repo.Save(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public void FindAll_Customers_Successfully()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => customerService.FindAll(Guid.NewGuid()));
    }

    [Fact]
    public void FindById_Customer_Successfully()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var mockCustomers =  MockCustomer.GetListFaker(3);
        
        var customerRepositoryMock = Usings.MockRepositorio(mockCustomers);
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);        

        var mockCustomer = MockCustomer.GetFaker();
        var customerId = mockCustomer.Id;
        mockCustomers.Add(mockCustomer);
        mockCustomer.Id = customerId;
        var customerDto = new CustomerDto()
        {
            Name = mockCustomer.Name,
            Email = mockCustomer.Login.Email,
            Password = mockCustomer.Login.Password,
            CPF = mockCustomer.CPF,
            Birth = mockCustomer.Birth,
            Phone = mockCustomer.Phone.Number,
            Address = new AddressDto
            {
                Zipcode = mockCustomer.Addresses.Last().Zipcode,
                Street = mockCustomer.Addresses.Last().Street,
                Number = mockCustomer.Addresses.Last().Number,
                Neighborhood = mockCustomer.Addresses.Last().Neighborhood,
                City = mockCustomer.Addresses.Last().City,
                State = mockCustomer.Addresses.Last().State,
                Complement = mockCustomer.Addresses.Last().Complement,
                Country = mockCustomer.Addresses.Last().Country
            }
        };

        customerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(mockCustomer);
        mapperMock.Setup(mapper => mapper.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        // Act
        var result = customerService.FindById(customerId);

        // Assert
        customerRepositoryMock.Verify(repo => repo.GetById(customerId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<CustomerDto>(mockCustomer), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockCustomer.Name, result.Name);
    }

    [Fact]
    public void Update_Customer_Successfully()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => customerService.Update(new CustomerDto()));
    }

    [Fact]
    public void Delete_Customer_Successfully()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = Usings.MockRepositorio(new List<Customer>());
        var flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());

        var customerService = new CustomerService(mapperMock.Object, customerRepositoryMock.Object, flatRepositoryMock.Object);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => customerService.Delete(new CustomerDto()));
    }
}