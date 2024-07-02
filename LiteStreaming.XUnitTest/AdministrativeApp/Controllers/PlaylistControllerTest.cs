using LiteStreaming.AdministrativeApp.Models;
using Application.Streaming.Dto;
using LiteStreaming.AdministrativeApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LiteStreaming.XunitTest.__mock__.Admin;
using LiteStreaming.Application.Abstractions;
using Microsoft.Data.SqlClient;

namespace AdministrativeApp.Controllers;

public class PlaylistControllerTest
{
    private readonly Mock<IService<PlaylistDto>> playlistServiceMock;
    private readonly PlaylistController playlistController;

    public PlaylistControllerTest()
    {
        playlistServiceMock = new Mock<IService<PlaylistDto>>();
        playlistController = new PlaylistController(playlistServiceMock.Object)
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
        var playlistsDto = MockPlaylist.Instance.GetDtoListFromPlaylistList(MockPlaylist.Instance.GetListFaker());
        playlistServiceMock.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(playlistsDto);

        // Act
        var result = playlistController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagerModel>(viewResult.Model);
        Assert.Equal(playlistsDto, model.GetItems<PlaylistDto>());
    }

    [Fact]
    public void Create_Returns_ViewResult()
    {
        // Act
        var result = playlistController.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewName);
    }

    [Fact]
    public void Save_ModelStateInvalid_Returns_CreateView()
    {
        // Arrange
        playlistController.ModelState.AddModelError("test", "test error");

        // Act
        var result = playlistController.Save(new PlaylistDto());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
    }

    [Fact]
    public void Save_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockPlaylist.Instance.GetFakerDto();        
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);

        // Act
        var result = playlistController.Save(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        playlistServiceMock.Verify(service => service.Create(dto), Times.Once);
    }

    [Fact]
    public void Save_ServiceThrowsArgumentException_Returns_CreateView_WithWarningAlert()
    {
        // Arrange
        var dto = MockPlaylist.Instance.GetFakerDto();
        
        playlistServiceMock.Setup(service => service.Create(It.IsAny<PlaylistDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);
        var result = playlistController.Save(dto);

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
        var dto = MockPlaylist.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);
        playlistServiceMock.Setup(service => service.Create(It.IsAny<PlaylistDto>())).Throws(new Exception("Error"));

        // Act
        var result = playlistController.Save(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao salvar os dados da playlista.", alert.Message);
    }

    [Fact]
    public void Update_ModelStateInvalid_Returns_EditView()
    {
        // Arrange
        playlistController.ModelState.AddModelError("test", "test error");
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", playlistController);

        PlaylistDto? nullplaylistDto = null;
        // Act
        var result = playlistController.Update(nullplaylistDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
    }

    [Fact]
    public void Update_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var dto = MockPlaylist.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);

        // Act
        var result = playlistController.Update(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        playlistServiceMock.Verify(service => service.Update(dto), Times.Once);
    }

    [Fact]
    public void Update_ServiceThrowsArgumentException_Returns_EditView_WithWarningAlert()
    {
        // Arrange
        var dto = MockPlaylist.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);
        playlistServiceMock.Setup(service => service.Update(It.IsAny<PlaylistDto>())).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = playlistController.Update(dto);

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
        var dto = MockPlaylist.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(dto.UsuarioId, "teste", "teste@teste.com", playlistController);
        playlistServiceMock.Setup(service => service.Update(It.IsAny<PlaylistDto>())).Throws(new Exception("Error"));

        // Act
        var result = playlistController.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao atualizar a playlista {dto?.Name}.", alert.Message);
    }

    [Fact]
    public void Edit_ValidId_Returns_ViewResult_With_Model()
    {
        // Arrange        
        var playlistDto = MockPlaylist.Instance.GetFakerDto();
        var id = playlistDto.Id;
        playlistServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Returns(playlistDto);

        // Act
        var result = playlistController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PlaylistDto>(viewResult.Model);
        Assert.Equal(playlistDto, model);
    }

    [Fact]
    public void Edit_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        playlistServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = playlistController.Edit(id);

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
        playlistServiceMock.Setup(service => service.FindById(It.IsAny<Guid>())).Throws(new Exception("Error"));

        // Act
        var result = playlistController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao editar os dados desta playlista.", alert.Message);
    }

    [Fact]
    public void Delete_ValidId_Returns_IndexView_With_SuccessAlert()
    {
        // Arrange
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", playlistController);
        var playlistDto = MockPlaylist.Instance.GetFakerDto();
        MockHttpContextHelper.MockClaimsIdentitySigned(playlistDto.UsuarioId, "teste", "teste@teste.com", playlistController);
        playlistServiceMock.Setup(service => service.Delete(It.IsAny<PlaylistDto>())).Returns(true);
        
        // Act
        var result = playlistController.Delete(playlistDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Sucesso", alert.Header);
        Assert.Equal("success", alert.Type);
        Assert.Equal($"Playlista {playlistDto?.Name} excluída.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        playlistServiceMock.Setup(service => service.Delete(It.IsAny<PlaylistDto>())).Throws(new ArgumentException("Invalid user."));
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", playlistController);

        // Act
        var result = playlistController.Delete(new());

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
        MockHttpContextHelper.MockClaimsIdentitySigned(Guid.NewGuid(), "teste", "teste@teste.com", playlistController);
        playlistServiceMock.Setup(service => service.Delete(It.IsAny<PlaylistDto>())).Throws(new Exception("Error"));

        // Act
        var result = playlistController.Delete(new());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao excluir a playlista .", alert.Message);
    }
}