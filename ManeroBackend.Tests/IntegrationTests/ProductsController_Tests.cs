using ManeroBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ManeroBackend.Contexts;
using ManeroBackend.Controllers;
using ManeroBackend.Repositories;
using ManeroBackend.Services;

namespace ManeroBackend.Tests.IntegrationTests;

public class ProductsController_Tests
{
    private readonly ProductContext _context;
    private readonly IProductService _productService;
    private readonly IProductRepository _productRepository;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly ProductsController _controller;

    public ProductsController_Tests()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProductContext(options);
        _productRepository = new ProductRepository(_context);

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _productService = new ProductService(_productRepository, _categoryRepositoryMock.Object);

        var categoryServiceMock = new Mock<ICategoryService>();
        _controller = new ProductsController(_productService, categoryServiceMock.Object);
    }

    [Fact]
    public async Task Create_Should_ReturnConflict_When_ProductAlreadyExists()
    {
        // Arrange
        var schema = new ProductSchema() { Name = "Product 1", CategoryName = "Category 1" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync(schema.CategoryName)).ReturnsAsync(new Category());
        await _productService.CreateAsync(request);

        // Act
        var result = await _controller.Create(schema);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ConflictResult>(result);

        await DisposeAsync();
    }


    private async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        _context.Dispose();
    }
}
