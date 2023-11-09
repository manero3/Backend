using ManeroBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ManeroBackend.Controllers;
using ManeroBackend.Services;

namespace ManeroBackend.Tests.UnitTests;

public class ProductsController_Tests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly ProductsController _controller;
    
    public ProductsController_Tests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new ProductsController(_mockProductService.Object, _mockCategoryService.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnBadRequest_When_ModelStateIsNotValid()
    {
        // Arrange
        var schema = new ProductSchema();
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.Create(schema);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnStatusCode500_When_Error()
    {
        // Arrange
        var schema = new ProductSchema();
        var request = new ServiceRequest<ProductSchema> { Content = schema};
        var response = new ServiceResponse<Product>
        {
            StatusCode = Enums.StatusCode.InternalServerError,
            Content = null
        };
        _mockProductService.Setup(x => x.CreateAsync(request)).ReturnsAsync(response);

        // Act
        var reslut = await _controller.Create(schema);


        // Assert
        Assert.IsType<ObjectResult>(reslut);

        var objectResult = reslut as ObjectResult;
        Assert.Equal(500, (int)objectResult!.StatusCode!);
        
    }

}
