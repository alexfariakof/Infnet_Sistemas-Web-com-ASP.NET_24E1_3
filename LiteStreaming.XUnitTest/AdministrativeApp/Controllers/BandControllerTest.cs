using LiteStreaming.AdministrativeApp.Models;
using Application.Streaming.Dto;
using LiteStreaming.AdministrativeApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LiteStreaming.XunitTest.__mock__.Admin;
using LiteStreaming.Application.Abstractions;
using Microsoft.Data.SqlClient;

namespace AdministrativeApp.Controllers;

public class BandControllerTest
{
    private readonly Mock<IService<BandDto>> bandServiceMock;
    private readonly BandController bandController;

    public BandControllerTest()
    {
        bandServiceMock = new Mock<IService<BandDto>>();
        bandController = new BandController(bandServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = Usings.MockHttpContext(),
            }
        };
    }

    [Fact]
    public void Index_Returns_ViewResult_With_Model()
    {
        // Arrange
        var bandsDto = MockBand.Instance.GetDtoListFromBandList(MockBand.Instance.GetListFaker());
        bandServiceMock.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(bandsDto);

        // Act
        var result = bandController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagerModel>(viewResult.Model);
        Assert.Equal(bandsDto, model.GetItems<BandDto>());
    }

    [Fact]
    public void Create_Returns_ViewResult()
    {
        // Act
        var result = bandController.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewName);
    }

    [Fact]
    public void Save_ModelStateInvalid_Returns_CreateView()
    {
        // Arrange
        bandController.ModelState.AddModelError("test", "test error");

        // Act
        var result = bandController.Save(new BandDto());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
    }

    [Fact]
    public void Save_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();        
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);

        // Act
        var result = bandController.Save(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        bandServiceMock.Verify(service => service.Create(dto), Times.Once);
    }

    [Fact]
    public void Save_ServiceThrowsArgumentException_Returns_CreateView_WithWarningAlert()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();
        
        bandServiceMock.Setup(service => service.Create(It.IsAny<BandDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);
        var result = bandController.Save(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Aviso", alert.Header);
        Assert.Equal("warning", alert.Type);
        Assert.Equal("Invalid data.", alert.Message);
    }

    [Fact]
    public void Save_ServiceThrowsException_Returns_CreateView_WithDangerAlert()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);
        bandServiceMock.Setup(service => service.Create(It.IsAny<BandDto>())).Throws(new Exception("Error"));

        // Act
        var result = bandController.Save(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao salvar os dados da banda.", alert.Message);
    }

    [Fact]
    public void Update_ModelStateInvalid_Returns_EditView()
    {
        // Arrange
        bandController.ModelState.AddModelError("test", "test error");
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", bandController);

        BandDto? nullbandDto = null;
        // Act
        var result = bandController.Update(nullbandDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
    }

    [Fact]
    public void Update_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);

        // Act
        var result = bandController.Update(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        bandServiceMock.Verify(service => service.Update(dto), Times.Once);
    }

    [Fact]
    public void Update_ServiceThrowsArgumentException_Returns_EditView_WithWarningAlert()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);
        bandServiceMock.Setup(service => service.Update(It.IsAny<BandDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = bandController.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Aviso", alert.Header);
        Assert.Equal("warning", alert.Type);
        Assert.Equal("Invalid data.", alert.Message);
    }

    [Fact]
    public void Update_ServiceThrowsException_Returns_EditView_WithDangerAlert()
    {
        // Arrange
        var dto = MockBand.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", bandController);
        bandServiceMock.Setup(service => service.Update(It.IsAny<BandDto>())).Throws(new Exception("Error"));

        // Act
        var result = bandController.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao atualizar a banda {dto?.Name}.", alert.Message);
    }

    [Fact]
    public void Edit_ValidId_Returns_ViewResult_With_Model()
    {
        // Arrange        
        var bandDto = MockBand.Instance.GetFakerDto();
        var id = bandDto.Id;
        bandServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Returns(bandDto);

        // Act
        var result = bandController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<BandDto>(viewResult.Model);
        Assert.Equal(bandDto, model);
    }

    [Fact]
    public void Edit_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        bandServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = bandController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Aviso", alert.Header);
        Assert.Equal("warning", alert.Type);
        Assert.Equal("Invalid user.", alert.Message);
    }

    [Fact]
    public void Edit_ServiceThrowsException_Returns_IndexView_WithDangerAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        bandServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new Exception("Error"));

        // Act
        var result = bandController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao editar os dados desta banda.", alert.Message);
    }

    [Fact]
    public void Delete_ValidId_Returns_IndexView_With_SuccessAlert()
    {
        // Arrange
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", bandController);
        var bandDto = MockBand.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(bandDto.UsuarioId, "teste", "teste@teste.com", bandController);
        bandServiceMock.Setup(service => service.Delete(It.IsAny<BandDto>())).Returns(true);
        
        // Act
        var result = bandController.Delete(bandDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Sucesso", alert.Header);
        Assert.Equal("success", alert.Type);
        Assert.Equal($"Banda {bandDto?.Name} excluída.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        bandServiceMock.Setup(service => service.Delete(It.IsAny<BandDto>())).Throws(new ArgumentException("Invalid user."));
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", bandController);

        // Act
        var result = bandController.Delete(new());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Aviso", alert.Header);
        Assert.Equal("warning", alert.Type);
        Assert.Equal("Invalid user.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsException_Returns_IndexView_WithDangerAlert()
    {
        // Arrange
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", bandController);
        bandServiceMock.Setup(service => service.Delete(It.IsAny<BandDto>())).Throws(new Exception("Error"));

        // Act
        var result = bandController.Delete(new());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao excluir a banda .", alert.Message);
    }
}