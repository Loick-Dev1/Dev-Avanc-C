using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class ProductAsyncControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryProductRepositoryAsync _repo;
        public ProductAsyncControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryProductRepositoryAsync)factory.Services.GetRequiredService<InMemoryProductRepositoryAsync>();
        }

        [Fact]

        public async Task ChangePrice_Should_Return_No_Content_And_Save_Product()
        {
            //Arrange
            var product = new Product();
            product.ChangePrice(10);
            _repo.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 20 };

            //Act
            var response = await _client.PutAsJsonAsync(
                $"/api/products/productasync/{product.Id}/price",
                request
            );

            //Assert - HTTP
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode );

            //Assert - Persistance réelle
            var updated = await _repo.GetByIdAsync(product.Id);
            Assert.Equal(20, updated!.Price);
        }
    }
}
