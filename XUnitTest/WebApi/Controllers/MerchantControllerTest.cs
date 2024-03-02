﻿using Application;
using Application.Account.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers;
public class MerchantControllerTest
{
    private Mock<IService<MerchantDto>> mockMerchantService;
    private MerchantController mockController;
    private void SetupBearerToken(Guid userId)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
        var identity = new ClaimsIdentity(claims, "UserId");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers["Authorization"] =
            "Bearer " + Usings.GenerateJwtToken(userId, "Merchant");

        mockController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }
    public MerchantControllerTest()
    {
        mockMerchantService = new Mock<IService<MerchantDto>>();
        mockController = new MerchantController(mockMerchantService.Object);
    }

    [Fact]
    public void FindById_Returns_Ok_Result_When_Merchant_Found()
    {
        // Arrange        
        var expectedMerchantDto = MockMerchant.GetDtoFromMerchant(MockMerchant.GetFaker());
        SetupBearerToken(expectedMerchantDto.Id);
        mockMerchantService.Setup(service => service.FindById(expectedMerchantDto.Id)).Returns(expectedMerchantDto);
        
        // Act
        var result = mockController.FindById() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MerchantDto>(result.Value);
        Assert.Equal(expectedMerchantDto, result.Value);
    }

    [Fact]
    public void FindById_Returns_NotFound_Result_When_Merchant_Not_Found()
    {
        // Arrange        
        var expectedMerchantDto = MockMerchant.GetDtoFromMerchant(MockMerchant.GetFaker());
        SetupBearerToken(expectedMerchantDto.Id);
        mockMerchantService.Setup(service => service.FindById(expectedMerchantDto.Id)).Returns((MerchantDto)null);

        // Act
        var result = mockController.FindById() as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var validMerchantDto  = MockMerchant.GetDtoFromMerchant(MockMerchant.GetFaker());
        SetupBearerToken(validMerchantDto.Id);
        mockMerchantService.Setup(service => service.Create(validMerchantDto)).Returns(validMerchantDto);

        // Act
        var result = mockController.Create(validMerchantDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MerchantDto>(result.Value);
        Assert.Equal(validMerchantDto, result.Value);
    }

    [Fact]
    public void Create_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        mockController.ModelState.AddModelError("errorKey", "ErrorMessage");
        // Act
        var result = mockController.Create(It.IsAny<MerchantDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Update_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var validMerchantDto = MockMerchant.GetDtoFromMerchant(MockMerchant.GetFaker());
        SetupBearerToken(validMerchantDto.Id);
        mockMerchantService.Setup(service => service.Update(validMerchantDto)).Returns(validMerchantDto);
        
        // Act
        var result = mockController.Update(validMerchantDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<MerchantDto>(result.Value);
        Assert.Equal(validMerchantDto, result.Value);
    }

    [Fact]
    public void Update_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange      
        SetupBearerToken(Guid.NewGuid());
        mockController.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = mockController.Update(It.IsAny<MerchantDto>()) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Delete_Returns_Ok_Result_When_ModelState_Is_Valid()
    {
        // Arrange        
        var mockMerchantDto = MockMerchant.GetDtoFromMerchant(MockMerchant.GetFaker());
        SetupBearerToken(mockMerchantDto.Id);
        mockMerchantService.Setup(service => service.Delete(It.IsAny<MerchantDto>())).Returns(true);
        mockMerchantService.Setup(service => service.FindById(mockMerchantDto.Id)).Returns(mockMerchantDto);
        
        // Act
        var result = mockController.Delete(mockMerchantDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)result.Value);
        mockMerchantService.Verify(b => b.Delete(It.IsAny<MerchantDto>()), Times.Once);
    }

    [Fact]
    public void Delete_Returns_BadRequest_Result_When_ModelState_Is_Invalid()
    {
        // Arrange
        SetupBearerToken(Guid.NewGuid());
        mockController.ModelState.AddModelError("errorKey", "ErrorMessage");

        // Act
        var result = mockController.Delete((MerchantDto)null);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
        mockMerchantService.Verify(b => b.Delete(It.IsAny<MerchantDto>()), Times.Never);
    }
}