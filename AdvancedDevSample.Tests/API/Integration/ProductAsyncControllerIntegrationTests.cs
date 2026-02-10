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

        private async Task AuthenticateAsync()
        {
            var loginRequest = new LoginRequest { Email = "john.doe@example.com", Password = "password" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse!.Token);
        }

        [Fact]
        public async Task ChangePrice_Should_Return_No_Content_And_Save_Product()
        {
            await AuthenticateAsync();
            //Arrange
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid());
            product.ChangePrice(10);
            _repo.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 20 };

            //Act
            var response = await _client.PatchAsJsonAsync(
                $"/api/products/{product.Id}/price",
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
