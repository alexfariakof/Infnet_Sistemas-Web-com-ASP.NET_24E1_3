using __mock__.Admin;
using Application.Administrative.Dto;
using Application.Shared.Dto;
using AutoMapper;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;
using EasyCryptoSalt;
using Microsoft.Extensions.Options;
using Moq;
using Repository.Persistency.Abstractions.Interfaces;
using System.Linq.Expressions;

namespace Application.Administrative;

public class AdministrativeAccountServiceTest
{
    private Mock<IMapper> mapperMock;
    private Mock<IRepository<AdminAccount>> administrativeAccountRepositoryMock;
    private readonly AdminAccountService administrativeAccountService;
    private readonly List<AdminAccount> mockAccountList = MockAdminAccount.Instance.GetListFaker(10);
    public AdministrativeAccountServiceTest()
    {
        var options = Options.Create(new CryptoOptions
        {
            Key = "[`T,Uj0$zse#_zF=[^*0>|-mYf/uHX=",
            AuthSalt = "j!SRTGE}46aSb$]R|jjTtKGY`|M<}yT+]W3E}"
        });

        var cryptoMock = new Crypto(options);
        mapperMock = new Mock<IMapper>();        
        administrativeAccountRepositoryMock = new Mock<IRepository<AdminAccount>>();
        administrativeAccountService = new AdminAccountService(mapperMock.Object, administrativeAccountRepositoryMock.Object, cryptoMock);
    }

    [Fact]
    public void Create_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdminAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdminAccount.Instance.GetFakerDto(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns(false);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns((new List<AdminAccount> {account }).AsEnumerable());        
        mapperMock.Setup(mapper => mapper.Map<AdminAccount>(accountDto)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdminAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Create(accountDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<AdminAccount, bool>>>()), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>()), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Save(account), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }

    [Fact]
    public void FindById_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdminAccount.Instance.GetFaker();
        var accountId = account.Id;
        var accountDto = MockAdminAccount.Instance.GetFakerDto(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.GetById(accountId)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdminAccountDto>(account)).Returns(accountDto);

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
        var accountDtos = MockAdminAccount.Instance.GetFakerListDto(accounts);
        administrativeAccountRepositoryMock.Setup(repo => repo.FindAll()).Returns(accounts.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<List<AdminAccountDto>>(It.IsAny<List<AdminAccount>>())).Returns(accountDtos);

        // Act
        var result = administrativeAccountService.FindAll(userId);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Update_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdminAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdminAccount.Instance.GetFakerDto(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns((new List<AdminAccount> { account }).AsEnumerable());
        mapperMock.Setup(mapper => mapper.Map<AdminAccount>(accountDto)).Returns(account);
        mapperMock.Setup(mapper => mapper.Map<AdminAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Update(accountDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Update(account), Times.Once);
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }

    [Fact]
    public void Delete_AdministrativeAccount_Successfully()
    {
        // Arrange
        var account = MockAdminAccount.Instance.GetFaker();
        account.PerfilType = Perfil.UserType.Admin;
        var accountDto = MockAdminAccount.Instance.GetFakerDto(account);
        administrativeAccountRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns(false);
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns((new List<AdminAccount> { account }).AsEnumerable());
        mapperMock.Setup(mapper => mapper.Map<AdminAccount>(accountDto)).Returns(account);

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
        var accountDtos = MockAdminAccount.Instance.GetFakerListDto(accounts);

        administrativeAccountRepositoryMock.Setup(repo => repo.FindAll()).Returns(accounts.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<List<AdminAccountDto>>(accounts)).Returns(accountDtos);

        // Act
        var result = administrativeAccountService.FindAll();

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void Authentication_Successfully()
    {
        // Arrange        
        var account = mockAccountList.First();        
        var accountDto = MockAdminAccount.Instance.GetFakerDto(account);
        account.Login.Password = accountDto.Password;
        var loginDto = new LoginDto { Email = accountDto.Email, Password = accountDto.Password };
        administrativeAccountRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>())).Returns(mockAccountList.AsQueryable());
        mapperMock.Setup(mapper => mapper.Map<AdminAccountDto>(account)).Returns(accountDto);

        // Act
        var result = administrativeAccountService.Authentication(loginDto);

        // Assert
        administrativeAccountRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<AdminAccount, bool>>>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(accountDto.Email, result.Email);
    }
}
