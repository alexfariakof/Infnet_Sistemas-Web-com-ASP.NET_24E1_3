using Application;
using Application.Account.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace WebApi.Controllers;
public class AlbumControllerTest
{
    private Mock<IService<AlbumDto>> mockAlbumService;
    private AlbumController controller;
    private void SetupBearerToken(Guid userId)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("UserType", "Album")
            };
        var identity = new ClaimsIdentity(claims, "UserId");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers.Authorization =
            "Bearer " + Usings.GenerateJwtToken(userId, "Customer");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    public AlbumControllerTest()
    {
        mockAlbumService = new Mock<IService<AlbumDto>>();
        controller = new AlbumController(mockAlbumService.Object);
    }

    [Fact]
    public void FindAll_Returns_Ok_Result_When_List_Album_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        var albumList = MockAlbum.Instance.GetListFaker(2);
        var expectedAlbumDtoList = MockAlbum.Instance.GetDtoListFromAlbumList(albumList);
        mockAlbumService.Setup(service => service.FindAll(userId)).Returns(expectedAlbumDtoList);
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<AlbumDto>>(result.Value);
        Assert.Equal(expectedAlbumDtoList, result.Value);
    }

    [Fact]
    public void FindAll_Returns_NotFound_Result_When_List_Album_Not_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        mockAlbumService.Setup(service => service.FindAll(userId)).Returns((List<AlbumDto>)null);
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void FindAll_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        mockAlbumService.Setup(service => service.FindAll(userId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void FindById_Returns_Ok_Result_When_Album_Found()
    {
        // Arrange        
        var album = MockAlbum.Instance.GetFaker();
        var expectedAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(album) ;
        mockAlbumService.Setup(service => service.FindById(album.Id)).Returns(expectedAlbumDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(album.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<AlbumDto>(result.Value);
        Assert.Equal(expectedAlbumDto, result.Value);
    }

    [Fact]
    public void FindById_Returns_NotFound_Result_When_Album_Not_Found()
    {
        // Arrange        
        var albumId = Guid.NewGuid();
        mockAlbumService.Setup(service => service.FindById(albumId)).Returns((AlbumDto)null);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(albumId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void FindById_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var albumId = Guid.NewGuid();
        mockAlbumService.Setup(service => service.FindById(albumId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(albumId) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Create_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var album = MockAlbum.Instance.GetFaker();
        var validAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(album);
        mockAlbumService.Setup(service => service.Create(validAlbumDto)).Returns(validAlbumDto);

        // Act
        var result = controller.Create(validAlbumDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<AlbumDto>(result.Value);
        Assert.Equal(validAlbumDto, result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange       
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Create(It.IsAny<AlbumDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var invalidAlbumDto = new AlbumDto(); // Invalid DTO to trigger exception in the service
        mockAlbumService.Setup(service => service.Create(invalidAlbumDto)).Throws(new Exception("BadRequest_Erro_Message"));

        // Act
        var result = controller.Create(invalidAlbumDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Update_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var validAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(MockAlbum.Instance.GetFaker());
        mockAlbumService.Setup(service => service.Update(validAlbumDto)).Returns(validAlbumDto);
        SetupBearerToken(Guid.NewGuid());
        // Act
        var result = controller.Update(validAlbumDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<AlbumDto>(result.Value);
        Assert.Equal(validAlbumDto, result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Update(It.IsAny<AlbumDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var validAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(MockAlbum.Instance.GetFaker());
        mockAlbumService.Setup(service => service.Update(validAlbumDto)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Update(validAlbumDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Delete_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var mockAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(MockAlbum.Instance.GetFaker());
        mockAlbumService.Setup(service => service.Delete(It.IsAny<AlbumDto>())).Returns(true);
        mockAlbumService.Setup(service => service.FindById(mockAlbumDto.Id)).Returns(mockAlbumDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockAlbumDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)result.Value);
        mockAlbumService.Verify(b => b.Delete(It.IsAny<AlbumDto>()), Times.Once);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Delete((AlbumDto)null);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
        mockAlbumService.Verify(b => b.Delete(It.IsAny<AlbumDto>()), Times.Never);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var mockAlbumDto = MockAlbum.Instance.GetDtoFromAlbum(MockAlbum.Instance.GetFaker());
        mockAlbumService.Setup(service => service.Delete(It.IsAny<AlbumDto>())).Throws(new Exception("BadRequest_Erro_Message"));
        mockAlbumService.Setup(service => service.FindById(mockAlbumDto.Id)).Returns(mockAlbumDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockAlbumDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }
}