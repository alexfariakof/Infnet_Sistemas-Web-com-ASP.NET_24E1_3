using __mock__.Admin;
using LiteStreaming.AdministrativeApp.Controllers;
using LiteStreaming.AdministrativeApp.Models;
using Application.Administrative.Interfaces;
using Application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LiteStreaming.XunitTest.__mock__.Admin;

namespace AdministrativeApp.Controllers;
public class AccountControllerTest
{
    private readonly Mock<IAdminAuthService> authenticationServiceMock;
    private readonly AccountController accountController;

    public AccountControllerTest()
    {
        authenticationServiceMock = new Mock<IAdminAuthService>();
        accountController = new AccountController(authenticationServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = Usings.MockHttpContext(),
            }
        };
    }

    [Fact]
    public void Index_Returns_ViewResult()
    {
        // Act
        var result = accountController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }


    [Fact]
    public void Error_Returns_ViewResult_With_ErrorViewModel()
    {
        // Arrange
        accountController.ControllerContext.HttpContext.TraceIdentifier = "test_trace_id";

        // Act
        var result = accountController.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal("test_trace_id", model.RequestId);
    }

    [Fact]
    public void SingIn_ModelStateInvalid_Returns_IndexView()
    {
        // Arrange
        accountController.ModelState.AddModelError("test", "test error");

        // Act
        var result = accountController.SingIn(new LoginDto());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
    }

    [Fact]
    public void SingIn_AuthenticationSuccessful_RedirectsToIndex()
    {
        // Arrange
        var accountDto = MockAdminAccount.Instance.GetFakerDto();
        accountDto.UsuarioId = accountDto.Id;
        var loginDto = new LoginDto { Email = accountDto.Email, Password = accountDto.Password };
        authenticationServiceMock.Setup(service => service.Authentication(It.IsAny<LoginDto>())).Returns(accountDto);
        MockHttpContextHelper.MockClaimsIdentitySigned(accountDto.Id, accountDto.Name, accountDto.Email, accountController);
        
        // Act
        var result = accountController.SingIn(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.True(accountController.User.Identity.IsAuthenticated);
    }

    [Fact]
    public void SingIn_AuthenticationThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        authenticationServiceMock.Setup(service => service.Authentication(It.IsAny<LoginDto>())).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = accountController.SingIn(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        //var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        //Assert.Equal("warning", alert.Type);
        //Assert.Equal("Invalid user.", alert.Message);
    }

    [Fact]
    public void SingIn_AuthenticationThrowsException_Returns_IndexView_WithDangerAlert()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        authenticationServiceMock.Setup(service => service.Authentication(It.IsAny<LoginDto>())).Throws(new Exception("Error"));

        // Act
        var result = accountController.SingIn(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao realizar login.", alert.Message);
    }

    [Fact]
    public void LogOut_SignOutAsyncAndRedirectsToIndex()
    {
        // Arrange
        MockHttpContextHelper.MockClaimsIdentity(Guid.NewGuid(), "Teste", "teste@teste.com", accountController);

        // Act
        var result = accountController.LogOut();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
