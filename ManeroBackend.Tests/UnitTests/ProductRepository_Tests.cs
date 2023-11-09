using Moq;
using System.Linq.Expressions;
using ManeroBackend.Repositories;
using ManeroBackend.Models.Entities;

namespace ManeroBackend.Tests.UnitTests;

public class ProductRepository_Tests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;


    public ProductRepository_Tests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnProductEntity_When_CreatedSuccessfully()
    {
        // Arrange
        var entity = new ProductEntity() { ArticleNumber = 1, Name = "Product 1", Description = "Product 1 Description" };
        _productRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(entity);

        // Act
        var result = await _productRepositoryMock.Object.CreateAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductEntity>(result);
        Assert.Equal(1, result.ArticleNumber);
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnTrue_When_EntityAlreadyExists()
    {
        // Arrange
        var entity = new ProductEntity() { ArticleNumber = 1, Name = "Product 1", Description = "Product 1 Description" };
        _productRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(true);

        // Act
        var result = await _productRepositoryMock.Object.ExistsAsync(x => x.ArticleNumber == 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnFalse_When_EntityDoNotExists()
    {
        // Arrange
        var entity = new ProductEntity() { ArticleNumber = 1, Name = "Product 1", Description = "Product 1 Description" };
        _productRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(false);

        // Act
        var result = await _productRepositoryMock.Object.ExistsAsync(x => x.ArticleNumber == 2);

        // Assert
        Assert.False(result);
    }
}
