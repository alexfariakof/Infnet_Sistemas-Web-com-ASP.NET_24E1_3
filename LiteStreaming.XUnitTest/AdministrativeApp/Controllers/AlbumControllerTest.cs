using LiteStreaming.AdministrativeApp.Controllers;
using LiteStreaming.AdministrativeApp.Models;
using Application.Streaming.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LiteStreaming.Application.Core.Interfaces.Query;
using LiteStreaming.XunitTest.__mock__.Admin;
using LiteStreaming.Application.Abstractions;
using Microsoft.Data.SqlClient;

namespace AdministrativeApp.Controllers;

public class AlbumControllerTest
{
    private readonly Mock<IService<AlbumDto>> albumServiceMock;
    private readonly Mock<IFindAll<BandDto>> bandServiceMock;
    private readonly Mock<IFindAll<GenreDto>> genreServiceMock;
    private readonly AlbumController albumController;

    public AlbumControllerTest()
    {
        albumServiceMock = new Mock<IService<AlbumDto>>();
        bandServiceMock = new Mock<IFindAll<BandDto>>();
        genreServiceMock = new Mock<IFindAll<GenreDto>>();

        albumController = new AlbumController(albumServiceMock.Object, bandServiceMock.Object, genreServiceMock.Object)
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
        var albumsDto = MockAlbum.Instance.GetDtoListFromAlbumList(MockAlbum.Instance.GetListFaker());
        albumServiceMock.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(albumsDto);

        // Act
        var result = albumController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagerModel>(viewResult.Model);
        Assert.Equal(albumsDto, model.GetItems<AlbumDto>());
    }

    [Fact]
    public void Create_Returns_ViewResult_With_Model()
    {
        // Act
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentity(Guid.NewGuid(), "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(s => s.FindAll()).Returns(new List<AlbumDto>() { viewModel.Album });
        bandServiceMock.Setup(s => s.FindAll()).Returns(viewModel.Bands);
        genreServiceMock.Setup(s => s.FindAll()).Returns(viewModel.Genres);

        var result = albumController.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AlbumViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.NotNull(model.Album);
        Assert.NotNull(model.Bands);
        Assert.NotNull(model.Genres);
    }

    [Fact]
    public void Save_ModelStateInvalid_Returns_CreateView()
    {
        // Arrange
        albumController.ModelState.AddModelError("test", "test error");

        // Act
        var result = albumController.Save(new AlbumViewModel());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
    }

    [Fact]
    public void Save_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Create(viewModel.Album));

        // Act
        var result = albumController.Save(viewModel);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        albumServiceMock.Verify(service => service.Create(viewModel.Album), Times.Once);
    }

    [Fact]
    public void Save_ServiceThrowsArgumentException_Returns_CreateView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Create(viewModel.Album)).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = albumController.Save(viewModel);

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
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);

        albumServiceMock.Setup(service => service.Create(viewModel.Album)).Throws(new Exception("Error"));

        // Act
        var result = albumController.Save(viewModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao salvar os dados do álbum.", alert.Message);
    }

    [Fact]
    public void Edit_ValidId_Returns_ViewResult_With_Model()
    {
        // Arrange        
        var albumDto = MockAlbum.Instance.GetFakerDto();
        var id = albumDto.Id;
        albumServiceMock.Setup(service => service.FindById(id)).Returns(albumDto);

        // Act
        var result = albumController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AlbumViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.Equal(albumDto, model.Album);
    }

    [Fact]
    public void Edit_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        albumServiceMock.Setup(service => service.FindById(id)).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = albumController.Edit(id);

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
        albumServiceMock.Setup(service => service.FindById(id)).Throws(new Exception("Error"));

        // Act
        var result = albumController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao editar os dados deste álbum.", alert.Message);
    }

    [Fact]
    public void Update_ModelStateInvalid_Returns_EditView()
    {
        // Arrange
        albumController.ModelState.AddModelError("test", "test error");

        // Act
        var result = albumController.Update(new AlbumViewModel());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
    }

    [Fact]
    public void Update_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Update(viewModel.Album));

        // Act
        var result = albumController.Update(viewModel);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        albumServiceMock.Verify(service => service.Update(viewModel.Album), Times.Once);
    }

    [Fact]
    public void Update_ServiceThrowsArgumentException_Returns_EditView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Update(viewModel.Album)).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = albumController.Update(viewModel);

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
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Update(viewModel.Album)).Throws(new Exception("Error"));

        // Act
        var result = albumController.Update(viewModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao atualizar o álbum {viewModel.Album?.Name}.", alert.Message);
    }

    [Fact]
    public void Delete_ValidId_Returns_IndexView_With_SuccessAlert()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Delete(viewModel.Album)).Returns(true);

        // Act
        var result = albumController.Delete(viewModel.Album);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Sucesso", alert.Header);
        Assert.Equal("success", alert.Type);
        Assert.Equal($"Álbum {viewModel.Album?.Name} excluído.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);
        albumServiceMock.Setup(service => service.Delete(viewModel.Album)).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = albumController.Delete(viewModel.Album);

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
        var viewModel = MockAlbumViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Album.UsuarioId, "teste", "teste@teste.com", albumController);

        albumServiceMock.Setup(service => service.Delete(viewModel.Album)).Throws(new Exception("Error"));

        // Act
        var result = albumController.Delete(viewModel.Album);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao excluir o álbum {viewModel.Album?.Name}.", alert.Message);
    }
}
