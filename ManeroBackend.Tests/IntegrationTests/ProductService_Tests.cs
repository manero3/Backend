using ManeroBackend.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using ManeroBackend.Contexts;
using ManeroBackend.Repositories;
using ManeroBackend.Services;

namespace ManeroBackend.Tests.IntegrationTests;

public class ProductService_Tests
{
    private readonly ProductContext _context;
    private readonly IProductService _productService;
    private readonly IProductRepository _productRepository;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    public ProductService_Tests()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProductContext(options);
        _productRepository = new ProductRepository(_context);

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _productService = new ProductService(_productRepository, _categoryRepositoryMock.Object);
    }

    [Fact] // anppasat till kategorin.
    public async Task CreateAsync_Should_ReturnServiceResponeWithStatusCode201_When_CreatedSuccessfully()
    {
        // Arrange
        var schema = new ProductSchema() { Name = "Product 1", CategoryName = "Category 1" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync(schema.CategoryName)).ReturnsAsync(new Category());

        // Act
        var result = await _productService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceResponse<Product>>(result);
        Assert.Equal(201, (int)result.StatusCode);
        Assert.Equal(schema.Name, result.Content!.Name);
    }


    [Fact] // anppasat till kategorin.
    public async Task CreateAsync_Should_ReturnServiceResponeWithStatusCode409_When_EntityAlreadyExists()
    {
        // Arrange
        var schema = new ProductSchema() { Name = "Product 1", CategoryName = "Category 1" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync(schema.CategoryName)).ReturnsAsync(new Category());
        await _productService.CreateAsync(request);

        // Act
        var result = await _productService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceResponse<Product>>(result);
        Assert.Equal(409, (int)result.StatusCode);
        Assert.Null(result.Content);
    }

}
