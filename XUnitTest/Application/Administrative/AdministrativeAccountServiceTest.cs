﻿using __mock__.Admin;
using Application.Administrative.Dto;
using Application.Shared.Dto;
using AutoMapper;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;
using Moq;
using Repository.Interfaces;
using System.Linq.Expressions;

namespace Application.Administrative;

public class AdministrativeAccountServiceTest
{
    private Mock<IMapper> mapperMock;
    private Mock<IRepository<AdministrativeAccount>> administrativeAccountRepositoryMock;
    private readonly AdministrativeAccountService administrativeAccountService;
    private readonly List<AdministrativeAccount> mockAccountList = MockAdministrativeAccount.Instance.GetListFaker(10);
    public AdministrativeAccountServiceTest()
    {
        mapperMock = new Mock<IMapper>();
        administrativeAccountRepositoryMock = new Mock<IRepository<AdministrativeAccount>>();
        administrativeAccountService = new AdministrativeAccountService(mapperMock.Object, administrativeAccountRepositoryMock.Object);
    }

    [Fact]
    public void Create_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdministrativeAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdministrativeAccount.Instance.GetNewDtoFromAdministrativeAccount(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns(false);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns((new List<AdministrativeAccount> {account }).AsEnumerable());        
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccount>(accountDto)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Create(accountDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>()), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>()), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Save(account), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }

    [Fact]
    public void FindById_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdministrativeAccount.Instance.GetFaker();
        var accountId = account.Id;
        var accountDto = MockAdministrativeAccount.Instance.GetNewDtoFromAdministrativeAccount(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.FindById(accountId);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.GetById(accountId), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountId, result.Id);
        Assert.Equal(accountDto.Email, result.Email);
    }

    [Fact]
    public void FindAll_AdministrativeAccounts_Successfully()
    {
        // Arrange
        
        var accounts = mockAccountList.Take(3).ToList();
        var userId = accounts.First().Id;
        var accountDtos = MockAdministrativeAccount.Instance.GetDtoListFromAdministrativeAccountList(accounts);
        administrativeAccountRepositoryMock.Setup(repo => repo.GetAll()).Returns(accounts.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<List<AdministrativeAccountDto>>(It.IsAny<List<AdministrativeAccount>>())).Returns(accountDtos);

        // Act
        var result = administrativeAccountService.FindAll(userId);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Update_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdministrativeAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdministrativeAccount.Instance.GetNewDtoFromAdministrativeAccount(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns((new List<AdministrativeAccount> { account }).AsEnumerable());
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccount>(accountDto)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Update(accountDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Update(account), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }

    [Fact]
    public void Delete_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdministrativeAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdministrativeAccount.Instance.GetNewDtoFromAdministrativeAccount(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns(false);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns((new List<AdministrativeAccount> { account }).AsEnumerable());
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccount>(accountDto)).Returns(account);

        // Act
        var result = administrativeAccountService.Delete(accountDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Delete(account), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public void FindAll_AdministrativeAccounts_WithoutUserId_Successfully()
    {
        // Arrange
        var accounts = mockAccountList.Take(3).ToList();
        var accountDtos = MockAdministrativeAccount.Instance.GetDtoListFromAdministrativeAccountList(accounts);

        administrativeAccountRepositoryMock.Setup(repo => repo.GetAll()).Returns(accounts.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<List<AdministrativeAccountDto>>(accounts)).Returns(accountDtos);

        // Act
        var result = administrativeAccountService.FindAll();

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void Authentication_Successfully()
    {
        // Arrange        
        var account = mockAccountList.First();        
        var accountDto = MockAdministrativeAccount.Instance.GetNewDtoFromAdministrativeAccount(account);
        account.Login.Password = accountDto.Password;
        var loginDto = new LoginDto { Email = accountDto.Email, Password = accountDto.Password };
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>())).Returns(mockAccountList.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<AdministrativeAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Authentication(loginDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdministrativeAccount, bool>>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }
}
