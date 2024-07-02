using Application.Streaming.Dto;
using LiteStreaming.AdministrativeApp.Controllers.Abstractions;
using LiteStreaming.AdministrativeApp.Models;
using LiteStreaming.Application.Abstractions;
using LiteStreaming.XunitTest.__mock__.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Moq;
using __mock__.Admin;
using System.Security.Claims;

namespace AdministrativeApp.Controllers.Abstractions;

public class UnitControllerBaseTest
{
    private readonly Mock<IService<FlatDto>> mockService;

    public UnitControllerBaseTest()
    {
        mockService = new Mock<IService<FlatDto>>();
    }

    public class MockUnitControllerBase : UnitControllerBase<FlatDto>
    {
        public MockUnitControllerBase(IService<FlatDto> service) : base(service) { }

        public IActionResult TestAction(HttpContext httpContext)
        {
            var actionContext = new ActionContext(
                     httpContext,
                     new Microsoft.AspNetCore.Routing.RouteData(),
                     new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                this);

            OnActionExecuting(context);
            return context.Result;
        }
    }

    [Fact]
    public void UnitControllerBase_UserId_SuccessfullyRetrieved()
    {
        // Arrange
        var controller = new MockUnitControllerBase(mockService.Object);
        var account = MockAdminAccount.Instance.GetFaker();
        MockHttpContextHelper.MockClaimsIdentitySigned(account.Id, account.Name, account.Login.Email, controller);

        // Act
        var result = controller.TestAction(controller.HttpContext);

        // Assert
        Assert.Equal(account.Id, controller.UserId);
    }

    [Fact]
    public void UnitControllerBase_UserId_ThrowsArgumentNullException()
    {
        // Arrange
        var controller = new MockUnitControllerBase(mockService.Object);
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal();

        // Act
        var result = Assert.Throws<NullReferenceException>(() => controller.TestAction(httpContext));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void UnitControllerBase_OnActionExecuting_SignOut_And_Set_LoginError()
    {
        // Arrange
        var controller = new MockUnitControllerBase(mockService.Object);
        MockHttpContextHelper.MockClaimsIdentity(null, "", "", controller);

        // Act        
        var result = controller.TestAction(controller.HttpContext);

        // Assert
        Assert.NotNull(controller.ViewBag.LoginError);
    }

    [Fact]
    public void Index_When_SortExpression_IsNull_Returns_DefaultSortModel()
    {
        // Arrange
        var controller = new MockUnitControllerBase(mockService.Object);
        var mockLlistDto = MockFlat.Instance.GetListDtoFaker();
        mockService.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(mockLlistDto.OrderBy(flat => flat.Name).ToList());
        string? sortExpression = null;

        // Act
        var result = controller.Index(sortExpression) as ViewResult;
        var pagerModel = result.Model as PagerModel;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(pagerModel);
        Assert.Equal(SortOrder.Ascending, pagerModel.SortModel.SortOrder);
        Assert.Equal("default_desc", pagerModel.SortModel.SortParamName);
        Assert.Equal(SortIcons.SORT_ICON_DESC, pagerModel.SortModel.SortIcon);
    }

    [Fact]
    public void Index_When_SortExpression_Does_Not_ContainDesc_Returns_Ascending_SortModel()
    {
        // Arrange
        var mockLlistDto = MockFlat.Instance.GetListDtoFaker();
        mockService.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(mockLlistDto.OrderBy(flat => flat.Name).ToList());
        var controller = new MockUnitControllerBase(mockService.Object);
        string sortExpression = "name";

        // Act
        var result = controller.Index(sortExpression) as ViewResult;
        var pagerModel = result.Model as PagerModel;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(pagerModel);
        Assert.Equal(SortOrder.Ascending, pagerModel.SortModel.SortOrder);
        Assert.Equal("name_desc", pagerModel.SortModel.SortParamName);
        Assert.Equal(SortIcons.SORT_ICON_DESC, pagerModel.SortModel.SortIcon);
    }

    [Fact]
    public void Index_When_SortExpression_ContainsDesc_Returns_Descending_SortModel()
    {
        // Arrange
        var mockLlistDto = MockFlat.Instance.GetListDtoFaker();
        mockService.Setup(service => service.FindAllOrdered(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<SortOrder>())).Returns(mockLlistDto.OrderByDescending(flat => flat.Name).ToList());
        var controller = new MockUnitControllerBase(mockService.Object);
        string sortExpression = "name_desc";

        // Act
        var result = controller.Index(sortExpression) as ViewResult;
        var pagerModel = result.Model as PagerModel;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(pagerModel);
        Assert.Equal(SortOrder.Descending, pagerModel.SortModel.SortOrder);
        Assert.Equal("name", pagerModel.SortModel.SortParamName);
        Assert.Equal(SortIcons.SORT_ICON_ASC, pagerModel.SortModel.SortIcon);
    }
}