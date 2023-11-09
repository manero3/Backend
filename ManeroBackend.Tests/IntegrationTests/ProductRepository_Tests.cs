using Microsoft.EntityFrameworkCore;
using ManeroBackend.Contexts;
using ManeroBackend.Repositories;
using ManeroBackend.Models.Entities;

namespace ManeroBackend.Tests.IntegrationTests;

public class ProductRepository_Tests
{
    private readonly ProductContext _context;
    private readonly IProductRepository _repository;

    public ProductRepository_Tests()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProductContext(options);
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_should_AddEntityToDatabase_And_ReturnEntity()
    {
        // Arrange
        var entity = new ProductEntity { Name = "Test Product" };

        // Act
        var result = await _repository.CreateAsync(entity);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ProductEntity>(result);
        Assert.Equal(entity.Name, result.Name);

        await DisposeAsync();
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnTrue_When_EntityAlreadyExists()
    {
        // Arrange
        var entity = new ProductEntity { Name = "Test Product" };
        await _repository.CreateAsync(entity);

        // Act
        var result = await _repository.ExistsAsync(x => x.Name == entity.Name);

        //Assert
        Assert.True(result);

        await DisposeAsync();
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnFalse_When_EntityDoesNotExists()
    {
        // Arrange
        var entity = new ProductEntity { Name = "Test Product" };

        // Act
        var result = await _repository.ExistsAsync(x => x.Name == entity.Name);

        //Assert
        Assert.False(result);

        await DisposeAsync();
    }



    private async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        _context.Dispose();
    }
}
