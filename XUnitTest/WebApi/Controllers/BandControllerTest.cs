using Application;
using Application.Account.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace WebApi.Controllers;
public class BandControllerTest
{
    private Mock<IService<BandDto>> mockBandService;
    private BandController controller;
    private void SetupBearerToken(Guid userId)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("UserType", "Band")
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

    public BandControllerTest()
    {
        mockBandService = new Mock<IService<BandDto>>();
        controller = new BandController(mockBandService.Object);
    }

    [Fact]
    public void FindAll_Returns_Ok_Result_When_List_Band_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        var bandList = MockBand.Instance.GetListFaker(2);
        var expectedBandDtoList = MockBand.Instance.GetDtoListFromBandList(bandList);
        mockBandService.Setup(service => service.FindAll(userId)).Returns(expectedBandDtoList);
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<BandDto>>(result.Value);
        Assert.Equal(expectedBandDtoList, result.Value);
    }

    [Fact]
    public void FindAll_Returns_NotFound_Result_When_List_Band_Not_Found()
    {
        // Arrange        
        var userId = Guid.NewGuid();
        mockBandService.Setup(service => service.FindAll(userId)).Returns((List<BandDto>)null);
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
        mockBandService.Setup(service => service.FindAll(userId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(userId);

        // Act
        var result = controller.FindAll() as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void FindById_Returns_Ok_Result_When_Band_Found()
    {
        // Arrange        
        var band = MockBand.Instance.GetFaker();
        var expectedBandDto = MockBand.Instance.GetDtoFromBand(band);
        mockBandService.Setup(service => service.FindById(band.Id)).Returns(expectedBandDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(band.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<BandDto>(result.Value);
        Assert.Equal(expectedBandDto, result.Value);
    }

    [Fact]
    public void FindById_Returns_NotFound_Result_When_Band_Not_Found()
    {
        // Arrange        
        var bandId = Guid.NewGuid();
        mockBandService.Setup(service => service.FindById(bandId)).Returns((BandDto)null);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(bandId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var band = MockBand.Instance.GetFaker();
        var validBandDto = MockBand.Instance.GetDtoFromBand(band);
        mockBandService.Setup(service => service.Create(validBandDto)).Returns(validBandDto);

        // Act
        var result = controller.Create(validBandDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<BandDto>(result.Value);
        Assert.Equal(validBandDto, result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange       
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Create(It.IsAny<BandDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Update_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var validBandDto = MockBand.Instance.GetDtoFromBand(MockBand.Instance.GetFaker());
        mockBandService.Setup(service => service.Update(validBandDto)).Returns(validBandDto);
        SetupBearerToken(Guid.NewGuid());
        // Act
        var result = controller.Update(validBandDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<BandDto>(result.Value);
        Assert.Equal(validBandDto, result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Update(It.IsAny<BandDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Delete_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var mockBandDto = MockBand.Instance.GetDtoFromBand(MockBand.Instance.GetFaker());
        mockBandService.Setup(service => service.Delete(It.IsAny<BandDto>())).Returns(true);
        mockBandService.Setup(service => service.FindById(mockBandDto.Id)).Returns(mockBandDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockBandDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)result.Value);
        mockBandService.Verify(b => b.Delete(It.IsAny<BandDto>()), Times.Once);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        controller.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = controller.Delete((BandDto)null);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
        mockBandService.Verify(b => b.Delete(It.IsAny<BandDto>()), Times.Never);
    }

    [Fact]
    public void FindById_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var bandId = Guid.NewGuid();
        mockBandService.Setup(service => service.FindById(bandId)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.FindById(bandId) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var invalidBandDto = new BandDto(); // Invalid DTO to trigger exception in the service
        mockBandService.Setup(service => service.Create(invalidBandDto)).Throws(new Exception("BadRequest_Erro_Message"));

        // Act
        var result = controller.Create(invalidBandDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var validBandDto = MockBand.Instance.GetDtoFromBand(MockBand.Instance.GetFaker());
        mockBandService.Setup(service => service.Update(validBandDto)).Throws(new Exception("BadRequest_Erro_Message"));
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Update(validBandDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_On_Exception()
    {
        // Arrange        
        var mockBandDto = MockBand.Instance.GetDtoFromBand(MockBand.Instance.GetFaker());
        mockBandService.Setup(service => service.Delete(It.IsAny<BandDto>())).Throws(new Exception("BadRequest_Erro_Message"));
        mockBandService.Setup(service => service.FindById(mockBandDto.Id)).Returns(mockBandDto);
        SetupBearerToken(Guid.NewGuid());

        // Act
        var result = controller.Delete(mockBandDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("BadRequest_Erro_Message", result.Value);
    }
}