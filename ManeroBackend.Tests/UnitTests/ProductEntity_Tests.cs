using ManeroBackend.Models.Entities;
using ManeroBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeroBackend.Tests.UnitTests
{
    public class ProductEntity_Tests
    {
        [Fact]
        public void ImplicitConversion_FromProductEntityToProduct_ShouldConvertCorrectly()
        {
            // Arrange
            var productEntity = new ProductEntity
            {
                ArticleNumber = 1,
                Name = "Product1",
                SupplierArticleNumber = "SupArt-85648",
                Description = "Description",
                ImageUrl = "http://example.com/image.png",
                Price = 99.99M
            };

            // Act
            Product product = productEntity;

            // Assert
            Assert.NotNull(product);
            Assert.Equal(productEntity.ArticleNumber, product.ArticleNumber);
            Assert.Equal(productEntity.Name, product.Name);
            Assert.Equal(productEntity.SupplierArticleNumber, product.SupplierArticleNumber);
            Assert.Equal(productEntity.Description, product.Description);
            Assert.Equal(productEntity.ImageUrl, product.ImageUrl);
            Assert.Equal(productEntity.Price, product.Price);
        }

        [Fact]
        public void ImplicitConversion_FromNullProductEntityToProduct_ShouldHandleNulls()
        {
            // Arrange
            ProductEntity productEntity = null;

            // Act & Assert
            var exception = Record.Exception(() => (Product)productEntity);
            Assert.Null(exception);
        }
    }
}


