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

public class MusicControllerTest
{
    private readonly Mock<IService<MusicDto>> musicServiceMock;
    private readonly Mock<IFindAll<BandDto>> bandServiceMock;
    private readonly Mock<IFindAll<GenreDto>> genreServiceMock;
    private readonly Mock<IFindAll<AlbumDto>> albumServiceMock;
    private readonly MusicController musicController;

    public MusicControllerTest()
    {
        musicServiceMock = new Mock<IService<MusicDto>>();
        bandServiceMock = new Mock<IFindAll<BandDto>>();
        genreServiceMock = new Mock<IFindAll<GenreDto>>();
        albumServiceMock = new Mock<IFindAll<AlbumDto>>();

        musicController = new MusicController(
            musicServiceMock.Object,
            genreServiceMock.Object,
            bandServiceMock.Object,
            albumServiceMock.Object)
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
        var musicsDto = MockMusic.Instance.GetDtoListFromMusicList(MockMusic.Instance.GetListFaker());
        musicServiceMock.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(musicsDto);

        // Act
        var result = musicController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagerModel>(viewResult.Model);
        Assert.Equal(musicsDto, model.GetItems<MusicDto>());
    }

    [Fact]
    public void Create_Returns_ViewResult_With_Model()
    {
        // Act
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentity(Guid.NewGuid(), "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(s => s.FindAll()).Returns(new List<MusicDto>() { viewModel.Music });
        bandServiceMock.Setup(s => s.FindAll()).Returns(viewModel.Bands);
        genreServiceMock.Setup(s => s.FindAll()).Returns(viewModel.Genres);
        albumServiceMock.Setup(s => s.FindAll()).Returns(viewModel.Albums);

        var result = musicController.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<MusicViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.NotNull(model.Music);
        Assert.NotNull(model.Bands);
        Assert.NotNull(model.Genres);
        Assert.NotNull(model.Albums);
    }

    [Fact]
    public void Save_ModelStateInvalid_Returns_CreateView()
    {
        // Arrange
        musicController.ModelState.AddModelError("test", "test error");

        // Act
        var result = musicController.Save(new MusicViewModel());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
    }

    [Fact]
    public void Save_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Create(viewModel.Music));

        // Act
        var result = musicController.Save(viewModel);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        musicServiceMock.Verify(service => service.Create(viewModel.Music), Times.Once);
    }

    [Fact]
    public void Save_ServiceThrowsArgumentException_Returns_CreateView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Create(viewModel.Music)).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = musicController.Save(viewModel);

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
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);

        musicServiceMock.Setup(service => service.Create(viewModel.Music)).Throws(new Exception("Error"));

        // Act
        var result = musicController.Save(viewModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Create", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao salvar os dados da musica.", alert.Message);
    }

    [Fact]
    public void Edit_ValidId_Returns_ViewResult_With_Model()
    {
        // Arrange        
        var musicDto = MockMusic.Instance.GetFakerDto();
        var id = musicDto.Id;
        musicServiceMock.Setup(service => service.FindById(id)).Returns(musicDto);

        // Act
        var result = musicController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<MusicViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.Equal(musicDto, model.Music);
    }

    [Fact]
    public void Edit_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var id = Guid.NewGuid();
        musicServiceMock.Setup(service => service.FindById(id)).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = musicController.Edit(id);

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
        musicServiceMock.Setup(service => service.FindById(id)).Throws(new Exception("Error"));

        // Act
        var result = musicController.Edit(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal("Ocorreu um erro ao editar os dados desta musica.", alert.Message);
    }

    [Fact]
    public void Update_ModelStateInvalid_Returns_EditView()
    {
        // Arrange
        musicController.ModelState.AddModelError("test", "test error");

        // Act
        var result = musicController.Update(new MusicViewModel());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
    }

    [Fact]
    public void Update_ValidDto_RedirectsToIndex()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Update(viewModel.Music));

        // Act
        var result = musicController.Update(viewModel);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        musicServiceMock.Verify(service => service.Update(viewModel.Music), Times.Once);
    }

    [Fact]
    public void Update_ServiceThrowsArgumentException_Returns_EditView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Update(viewModel.Music)).Throws(new ArgumentException("Invalid data."));

        // Act
        var result = musicController.Update(viewModel);

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
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Update(viewModel.Music)).Throws(new Exception("Error"));

        // Act
        var result = musicController.Update(viewModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao atualizar a musica {viewModel.Music?.Name}.", alert.Message);
    }

    [Fact]
    public void Delete_ValidId_Returns_IndexView_With_SuccessAlert()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Delete(viewModel.Music)).Returns(true);

        // Act
        var result = musicController.Delete(viewModel.Music);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Sucesso", alert.Header);
        Assert.Equal("success", alert.Type);
        Assert.Equal($"Musica {viewModel.Music?.Name} excluída.", alert.Message);
    }

    [Fact]
    public void Delete_ServiceThrowsArgumentException_Returns_IndexView_WithWarningAlert()
    {
        // Arrange
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);
        musicServiceMock.Setup(service => service.Delete(viewModel.Music)).Throws(new ArgumentException("Invalid user."));

        // Act
        var result = musicController.Delete(viewModel.Music);

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
        var viewModel = MockMusicViewModel.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(viewModel.Music.UsuarioId, "teste", "teste@teste.com", musicController);

        musicServiceMock.Setup(service => service.Delete(viewModel.Music)).Throws(new Exception("Error"));

        // Act
        var result = musicController.Delete(viewModel.Music);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var alert = Assert.IsType<AlertViewModel>(viewResult.ViewData["Alert"]);
        Assert.Equal("Erro", alert.Header);
        Assert.Equal("danger", alert.Type);
        Assert.Equal($"Ocorreu um erro ao excluir a musica {viewModel.Music?.Name}.", alert.Message);
    }
}
