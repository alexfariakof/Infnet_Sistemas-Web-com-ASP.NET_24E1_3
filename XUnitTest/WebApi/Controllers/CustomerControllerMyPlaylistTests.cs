using Application;
using Application.Account.Dto;
using Domain.Account.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace WebApi.Controllers;
public class CustomerControllerMyPlaylistTests
{
    private readonly Mock<IService<CustomerDto>> mockCustomerService;
    private readonly Mock<IService<PlaylistPersonalDto>> mockPlaylistService;
    private readonly CustomerController controller;
    private void SetupBearerToken(Guid userId, UserTypeEnum userType = UserTypeEnum.Customer)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, userType.ToString())
            };
        var identity = new ClaimsIdentity(claims, "UserId");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers.Authorization = "Bearer " + Usings.GenerateJwtToken(userId, userType.ToString());

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    public CustomerControllerMyPlaylistTests()
    {
        // Arrange
        mockCustomerService = new Mock<IService<CustomerDto>>();
        mockPlaylistService = new Mock<IService<PlaylistPersonalDto>>();
        controller = new CustomerController(mockCustomerService.Object, mockPlaylistService.Object);
    }

    [Fact]
    public void FindAllPlaylist_Returns_Ok_Result_When_Playlists_Found()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);

        var playlists = MockPlaylistPersonal.Instance.GetListFaker(3);
        var playlistsDto = MockPlaylistPersonal.Instance.GetDtoListFromPlaylistPersonalList(playlists);

        mockPlaylistService.Setup(service => service.FindAll(userIdentity)).Returns(playlistsDto);

        // Act
        var result = controller.FindAllPlaylist() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<PlaylistPersonalDto>>(result.Value);
        Assert.Equal(playlistsDto, result.Value);
    }

    [Fact]
    public void FindAllPlaylist_Returns_Unauthorized_Result_When_User_Not_Customer()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid(), UserTypeEnum.Merchant);
        // Act
        var result = controller.FindAllPlaylist() as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void FindByIdPlaylist_Returns_Ok_Result_When_Playlist_Found()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var playlistId = playlist.Id;
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity);

        mockPlaylistService.Setup(service => service.FindById(playlistId)).Returns(playlistDto);

        // Act
        var result = controller.FindByIdPlaylist(playlistId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PlaylistPersonalDto>(result.Value);
        Assert.Equal(playlistDto, result.Value);
    }

    [Fact]
    public void FindByIdPlaylist_Returns_NotFound_Result_When_Playlist_Not_Found()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);

        var playlistId = Guid.NewGuid();

        mockPlaylistService.Setup(service => service.FindById(playlistId)).Returns((PlaylistPersonalDto)null);

        // Act
        var result = controller.FindByIdPlaylist(playlistId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void FindByIdPlaylist_Returns_Unauthorized_Result_When_User_Not_Customer()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity, UserTypeEnum.Merchant);

        // Act
        var result = controller.FindByIdPlaylist(Guid.NewGuid()) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void CreatePlaylist_Returns_Unauthorized_Result_When_User_Not_Customer()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity, UserTypeEnum.Merchant);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);
        mockPlaylistService.Setup(service => service.Create(playlistDto)).Returns(playlistDto);

        // Act
        var result = controller.CreatePlaylist(playlistDto) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void CreatePlaylist_Returns_Ok_Result_When_Valid_Playlist()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);        
        mockPlaylistService.Setup(service => service.Create(playlistDto)).Returns(playlistDto);

        // Act
        var result = controller.CreatePlaylist(playlistDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PlaylistPersonalDto>(result.Value);
        Assert.Equal(playlistDto, result.Value);
    }

    [Fact]
    public void CreatePlaylist_Returns_BadRequest_Result_When_Invalid_Playlist()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = new PlaylistPersonalDto();

        // Act
        var result = controller.CreatePlaylist(playlistDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(result.Value as IEnumerable<string>);
    }

    [Fact]
    public void CreatePlaylist_Returns_BadRequest_Result_When_Playlist_Deletion_Fails()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(MockPlaylistPersonal.Instance.GetFaker());
        mockPlaylistService.Setup(service => service.Create(playlistDto)).Returns(() => throw new Exception("Failed to create the playlist."));

        // Act
        var result = controller.CreatePlaylist(playlistDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to create the playlist.", result.Value);
        mockPlaylistService.Verify(d => d.Delete(It.IsAny<PlaylistPersonalDto>()), Times.Never);
    }

    [Fact]
    public void UpdatePlaylist_Returns_Unauthorized_Result_When_User_Not_Customer()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity, UserTypeEnum.Merchant);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);
        mockPlaylistService.Setup(service => service.Update(playlistDto)).Returns(playlistDto);

        // Act
        var result = controller.UpdatePlaylist(playlistDto) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedResult>(result);
        mockPlaylistService.Verify(d => d.Delete(It.IsAny<PlaylistPersonalDto>()), Times.Never);
    }

    [Fact]
    public void UpdatePlaylist_Returns_Ok_Result_When_Valid_Playlist()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity);
        mockPlaylistService.Setup(service => service.Update(playlistDto)).Returns(playlistDto);

        // Act
        var result = controller.UpdatePlaylist(playlistDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PlaylistPersonalDto>(result.Value);
        Assert.Equal(playlistDto, result.Value);
        mockPlaylistService.Verify(d => d.Update(It.IsAny<PlaylistPersonalDto>()), Times.Once);
    }

    [Fact]
    public void UpdatePlaylist_Returns_BadRequest_Result_When_Invalid_Playlist()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = new PlaylistPersonalDto();

        // Act
        var result = controller.UpdatePlaylist(playlistDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(result.Value as IEnumerable<string>);
    }

    [Fact]
    public void UpdatePlaylist_Returns_BadRequest_Result_When_Playlist_Deletion_Fails()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(MockPlaylistPersonal.Instance.GetFaker());
        mockPlaylistService.Setup(service => service.Update(playlistDto)).Returns(() => throw new Exception("Failed to create the playlist."));

        // Act
        var result = controller.UpdatePlaylist(playlistDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to create the playlist.", result.Value);
        mockPlaylistService.Verify(d => d.Delete(It.IsAny<PlaylistPersonalDto>()), Times.Never);
    }

    [Fact]
    public void DeletePlaylist_Returns_Unauthorized_Result_When_User_Not_Customer()
    {
        // Arrange
        var playlist = MockPlaylistPersonal.Instance.GetFaker();
        var userIdentity = playlist.Customer.Id;
        SetupBearerToken(userIdentity, UserTypeEnum.Merchant);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(playlist);
        mockPlaylistService.Setup(service => service.Delete(playlistDto)).Returns(false);

        // Act
        var result = controller.DeletePlaylist(playlistDto) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedResult>(result);
        mockPlaylistService.Verify(d =>d .Delete(It.IsAny<PlaylistPersonalDto>()), Times.Never);
    }

    [Fact]
    public void DeletePlaylist_Returns_Ok_Result_When_Playlist_Deleted_Successfully()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(MockPlaylistPersonal.Instance.GetFaker());
        mockPlaylistService.Setup(service => service.Delete(playlistDto)).Returns(true);

        // Act
        var result = controller.DeletePlaylist(playlistDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<bool>(result.Value);
        Assert.True((bool)result.Value);
        mockPlaylistService.Verify(d => d.Delete(It.IsAny<PlaylistPersonalDto>()), Times.Once);
    }

    [Fact]
    public void DeletePlaylist_Returns_BadRequest_Result_When_Playlist_Deletion_Fails()
    {
        // Arrange
        var userIdentity = Guid.NewGuid();
        SetupBearerToken(userIdentity);
        var playlistDto = MockPlaylistPersonal.Instance.GetDtoFromPlaylistPersonal(MockPlaylistPersonal.Instance.GetFaker());
        mockPlaylistService.Setup(service => service.Delete(playlistDto)).Returns(() => throw new Exception("Failed to delete the playlist."));

        // Act
        var result = controller.DeletePlaylist(playlistDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to delete the playlist.", result.Value);
        mockPlaylistService.Verify(d => d.Delete(It.IsAny<PlaylistPersonalDto>()), Times.Once);
    }
}
