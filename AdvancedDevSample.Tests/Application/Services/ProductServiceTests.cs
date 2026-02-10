using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Tests.Application.Fakes;
using AdvancedDevSample.Domain.Entities;
using Xunit;
using System;

namespace AdvancedDevSample.Tests.Application.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public void ChangeProductPrice_Should_Save_Product_When_Price_Is_Valid()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), 10m, true); // état initial valide
            var repo = new FakeProductRepository(product);
            var service = new ProductService(repo);

            // Act
            var request = new ChangePriceRequest { NewPrice = 20m };
            service.ChangeProductPrice(product.Id, request.NewPrice);

            // Assert
            Assert.Equal(20m, product.Price);
            Assert.True(repo.WasSaved);
        }
    }
}
