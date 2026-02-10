using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Tests.Application.Fakes;
using AdvancedDevSample.Domain.Entities;
using Xunit;
using System;
using System.Threading.Tasks;

namespace AdvancedDevSample.Tests.Application.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task ChangeProductPrice_Should_Save_Product_When_Price_Is_Valid()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid()); // état initial valide
            var repo = new FakeProductRepository(product);
            // Passing null for IProviderRepository as it is not used in this test
            var service = new ProductService(repo, null!);

            // Act
            var request = new ChangePriceRequest { NewPrice = 20m };
            await service.ChangeProductPriceAsync(product.Id, request.NewPrice);

            // Assert
            Assert.Equal(20m, product.Price);
            Assert.True(repo.WasSaved);
        }
    }
}
