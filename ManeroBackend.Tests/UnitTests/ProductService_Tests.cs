using ManeroBackend.Models;
using Moq;
using ManeroBackend.Enums;
using ManeroBackend.Services;
using ManeroBackend.Models.Entities;

namespace ManeroBackend.Tests.UnitTests;

public class ProductService_Tests
{
    private readonly Mock<IProductService> _productServiceMock;


    public ProductService_Tests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnServiceResponseWithStatusCode201_When_CreatedSuccessfully()
    {
        // Arrange
        var schema = new ProductSchema() { Name = "Product 1", Description = "Product 1 Description" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        var entity = new ProductEntity() { ArticleNumber = 1, Name = "Product 1", Description = "Product 1 Description" };
        var response = new ServiceResponse<Product>
        {
            StatusCode = StatusCode.Created,
            Content = entity
        };

        _productServiceMock.Setup(x => x.CreateAsync(It.IsAny<ServiceRequest<ProductSchema>>())).ReturnsAsync(response);

        // Act
        var result = await _productServiceMock.Object.CreateAsync(request);

        // Assert
        Assert.NotNull(result.Content);
        Assert.Equal(StatusCode.Created, result.StatusCode);
    }


    [Fact]
    public async Task CreateAsync_Should_ReturnServiceResponseWithStatusCode409_When_ProductAlreadyExists()
    {
        // Arrange
        var schema = new ProductSchema() { Name = "Product 1", Description = "Product 1 Description" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        var response = new ServiceResponse<Product>
        {
            StatusCode = StatusCode.Conflict,
            Content = null
        };

        _productServiceMock.Setup(x => x.CreateAsync(It.IsAny<ServiceRequest<ProductSchema>>())).ReturnsAsync(response);

        // Act
        var result = await _productServiceMock.Object.CreateAsync(request);

        // Assert
        Assert.Null(result.Content);
        Assert.Equal(StatusCode.Conflict, result.StatusCode);
    }

}
