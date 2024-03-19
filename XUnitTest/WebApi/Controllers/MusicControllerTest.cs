using Application;
using Application.Account.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace WebApi.Controllers;
public class MusicControllerTest
{
    private Mock<IService<MusicDto>> mockMusicService;
    private MusicController controller;
    private void SetupBearerToken(Guid userId)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("UserType", "Music")
            };
        var identity = new ClaimsIdentity(claims, "UserId");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers.Authorization =
            "Bearer " + Usings.GenerateJwtToken(userId, "UserId");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    public MusicControllerTest()
    {
        mockMusicService = new Mock<IService<MusicDto>>();
        controller = new MusicController(mockMusicService.Object);
    }

    [Fact]
    public void FindAll_Returns_Ok_Result_When_List_Music_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        var musicList = MockMusic.Instance.GetListFaker(2);
        var expectedMusicDtoList = MockMusic.Instance.GetDtoListFromMusicList(musicList);
        mockMusicService.Setup(service => service.FindAll(userId)).Returns(expectedMusicDtoList);
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<MusicDto>>(result.Value);
        Assert.Equal(expectedMusicDtoList, result.Value);
    }

    [Fact]
    public void FindAll_Returns_NotFound_Result_When_List_Music_Not_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        mockMusicService.Setup(service => service.FindAll(userId)).Returns((List<MusicDto>)null);
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
        mockMusicService.Setup(service => service.FindAll(userId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void FindById_Returns_Ok_Result_When_Music_Found()
    {
        // Arrange        
        var music = MockMusic.Instance.GetFaker();
        var expectedMusicDto = MockMusic.Instance.GetDtoFromMusic(music) ;
        mockMusicService.Setup(service => service.FindById(music.Id)).Returns(expectedMusicDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(music.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MusicDto>(result.Value);
        Assert.Equal(expectedMusicDto, result.Value);
    }

    [Fact]
    public void FindById_Returns_NotFound_Result_When_Music_Not_Found()
    {
        // Arrange        
        var musicId = Guid.NewGuid();
        mockMusicService.Setup(service => service.FindById(musicId)).Returns((MusicDto)null);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(musicId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var music = MockMusic.Instance.GetFaker();
        var validMusicDto = MockMusic.Instance.GetDtoFromMusic(music);
        mockMusicService.Setup(service => service.Create(validMusicDto)).Returns(validMusicDto);

        // Act
        var result = controller.Create(validMusicDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MusicDto>(result.Value);
        Assert.Equal(validMusicDto, result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange       
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Create(It.IsAny<MusicDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Update_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var validMusicDto = MockMusic.Instance.GetDtoFromMusic(MockMusic.Instance.GetFaker());
        mockMusicService.Setup(service => service.Update(validMusicDto)).Returns(validMusicDto);
        SetupBearerToken(Guid.NewGuid());
        // Act
        var result = controller.Update(validMusicDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MusicDto>(result.Value);
        Assert.Equal(validMusicDto, result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Update(It.IsAny<MusicDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Delete_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var mockMusicDto = MockMusic.Instance.GetDtoFromMusic(MockMusic.Instance.GetFaker());
        mockMusicService.Setup(service => service.Delete(It.IsAny<MusicDto>())).Returns(true);
        mockMusicService.Setup(service => service.FindById(mockMusicDto.Id.Value)).Returns(mockMusicDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockMusicDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)result.Value);
        mockMusicService.Verify(b => b.Delete(It.IsAny<MusicDto>()), Times.Once);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Delete((MusicDto)null);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
        mockMusicService.Verify(b => b.Delete(It.IsAny<MusicDto>()), Times.Never);
    }

    [Fact]
    public void FindById_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var musicId = Guid.NewGuid();
        mockMusicService.Setup(service => service.FindById(musicId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(musicId) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var invalidMusicDto = new MusicDto(); // Invalid DTO to trigger exception in the service
        mockMusicService.Setup(service => service.Create(invalidMusicDto)).Throws(new Exception("BadRequest_Erro_Message"));

        // Act
        var result = controller.Create(invalidMusicDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var validMusicDto = MockMusic.Instance.GetDtoFromMusic(MockMusic.Instance.GetFaker());
        mockMusicService.Setup(service => service.Update(validMusicDto)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Update(validMusicDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var mockMusicDto = MockMusic.Instance.GetDtoFromMusic(MockMusic.Instance.GetFaker());
        mockMusicService.Setup(service => service.Delete(It.IsAny<MusicDto>())).Throws(new Exception("BadRequest_Erro_Message"));
        mockMusicService.Setup(service => service.FindById(mockMusicDto.Id.Value)).Returns(mockMusicDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockMusicDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }
}