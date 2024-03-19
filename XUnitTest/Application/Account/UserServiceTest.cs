using Application.Account.Dto;
using Application.Authentication;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Repository;
using System.Linq.Expressions;

namespace Application.Account;
public class UserServiceTest
{
    private Mock<IMapper> mapperMock;
    private Mock<IRepository<User>> userRepositoryMock;
    private readonly UserService userService;
    private readonly List<User> mockUserList = MockCustomer.Instance.GetListFaker(3).Select(u => u.User).ToList();
    private readonly Mock<ICrypto> cryptoMock;
    public UserServiceTest()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var signingConfigurations = new SigningConfigurations();
        configuration.GetSection("TokenConfigurations").Bind(signingConfigurations);

        var tokenConfigurations = new TokenConfiguration();
        configuration.GetSection("TokenConfigurations").Bind(tokenConfigurations);

        mapperMock = new Mock<IMapper>();
        cryptoMock = new Mock<ICrypto>();
        userRepositoryMock = Usings.MockRepositorio(mockUserList);

        userService = new UserService(
            mapperMock.Object,
            userRepositoryMock.Object,
            signingConfigurations,
            tokenConfigurations
        );
    }

    [Fact]
    public void Authentication_With_Valid_Credentials_Should_Return_AuthenticationDto()
    {
        // Arrange
        var mockUser = mockUserList.First();
        mockUser.Login.Password = "validPassword";
        var loginDto = new LoginDto { Email = mockUser.Login.Email, Password = "validPassword" };
        
        userRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(mockUserList.Where(u => u.Login.Email.Equals(mockUser.Login.Email)));
        cryptoMock.Setup(crypto => crypto.Decrypt(It.IsAny<string>())).Returns(mockUser.Login.Password);

        // Act
        var result = userService.Authentication(loginDto);

        // Assert
        userRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
    }

    [Fact]
    public void Authentication_With_Invalid_Credentials_Should_Throw_Exception()
    {
        // Arrange
        var mockUser = mockUserList.First();
        var loginDto = new LoginDto { Email = "invalid.email@example.com", Password = "invalidPassword" };
        userRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(mockUserList.Where(u => u.Login.Email.Equals(mockUser.Login.Email)));
        cryptoMock.Setup(crypto => crypto.Decrypt(It.IsAny<string>())).Returns(mockUser.Login.Password);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => userService.Authentication(loginDto));
        Assert.Equal("Usuário Inválido!", exception.Message);
        userRepositoryMock.Verify(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
    }
}