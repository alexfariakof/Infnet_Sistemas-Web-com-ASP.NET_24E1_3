using LiteStreaming.AdministrativeApp.Models;
using Application.Streaming.Dto;
using LiteStreaming.AdministrativeApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LiteStreaming.XunitTest.__mock__.Admin;
using LiteStreaming.Application.Abstractions;
using Microsoft.Data.SqlClient;

namespace AdministrativeApp.Controllers;

public class FlatControllerTest
{
    private readonly Mock<IService<FlatDto>> flatServiceMock;
    private readonly FlatController flatController;

    public FlatControllerTest()
    {
        flatServiceMock = new Mock<IService<FlatDto>>();
        flatController = new FlatController(flatServiceMock.Object)
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
        var flatsDto = MockFlat.Instance.GetDtoListFromFlatList(MockFlat.Instance.GetListFaker());
        flatServiceMock.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(flatsDto);

        // Act
        var result = flatController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagerModel>(viewResult.Model);
        Assert.Equal(flatsDto, model.GetItems<FlatDto>());
    }

    [Fact]
    public void Create_Returns_ViewResult()
    {
        // Act
        var result = flatController.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewName);
    }

    [Fact]
    public void Save_ModelStateInvalid_Returns_CreateView()
    {
        // Arrange
        flatController.ModelState.AddModelError("test", "test error");

        // Act
        var result = flatController.Save(new FlatDto());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
    }

    [Fact]
    public void Save_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockFlat.Instance.GetFakerDto();        
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);

        // Act
        var result = flatController.Save(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        flatServiceMock.Verify(service => service.Create(dto), Times.Once);
    }

    [Fact]
    public void Save_ServiceThrowsArgumentException_Returns_CreateView_WithWarningAlert()
    {
        // Arrange
        var dto = MockFlat.Instance.GetFakerDto();
        
        flatServiceMock.Setup(service => service.Create(It.IsAny<FlatDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);
        var result = flatController.Save(dto);

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
        var dto = MockFlat.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);
        flatServiceMock.Setup(service => service.Create(It.IsAny<FlatDto>())).Throws(new Exception("Error"));

        // Act
        var result = flatController.Save(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao salvar os dados do plano.", alert.Message);
    }

    [Fact]
    public void Update_ModelStateInvalid_Returns_EditView()
    {
        // Arrange
        flatController.ModelState.AddModelError("test", "test error");
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", flatController);

        FlatDto? nullflatDto = null;
        // Act
        var result = flatController.Update(nullflatDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
    }

    [Fact]
    public void Update_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockFlat.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);

        // Act
        var result = flatController.Update(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        flatServiceMock.Verify(service => service.Update(dto), Times.Once);
    }

    [Fact]
    public void Update_ServiceThrowsArgumentException_Returns_EditView_WithWarningAlert()
    {
        // Arrange
        var dto = MockFlat.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);
        flatServiceMock.Setup(service => service.Update(It.IsAny<FlatDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = flatController.Update(dto);

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
        var dto = MockFlat.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", flatController);
        flatServiceMock.Setup(service => service.Update(It.IsAny<FlatDto>())).Throws(new Exception("Error"));

        // Act
        var result = flatController.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao atualizar o plano {dto?.Name}.", alert.Message);
    }

    [Fact]
    public void Edit_ValidId_Returns_ViewResult_With_Model()
    {
        // Arrange        
        var flatDto = MockFlat.Instance.GetFakerDto();
        var id = flatDto.Id;
        flatServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Returns(flatDto);

        // Act
        var result = flatController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FlatDto>(viewResult.Model);
        Assert.Equal(flatDto, model);
    }

    [Fact]
    public void Edit_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        flatServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = flatController.Edit(id);

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
        flatServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new Exception("Error"));

        // Act
        var result = flatController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao editar os dados deste plano.", alert.Message);
    }

    [Fact]
    public void Delete_ValidId_Returns_IndexView_With_SuccessAlert()
    {
        // Arrange
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", flatController);
        var flatDto = MockFlat.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(flatDto.UsuarioId, "teste", "teste@teste.com", flatController);
        flatServiceMock.Setup(service => service.Delete(It.IsAny<FlatDto>())).Returns(true);
        
        // Act
        var result = flatController.Delete(flatDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Sucesso", alert.Header);
        Assert.Equal("success", alert.Type);
        Assert.Equal($"Plano { flatDto.Name } excluído.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        flatServiceMock.Setup(service => service.Delete(It.IsAny<FlatDto>())).Throws(new ArgumentException("Invalid user."));
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", flatController);

        // Act
        var result = flatController.Delete(new());

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
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", flatController);
        flatServiceMock.Setup(service => service.Delete(It.IsAny<FlatDto>())).Throws(new Exception("Error"));

        // Act
        var result = flatController.Delete(new());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao excluir o plano .", alert.Message);
    }
}
