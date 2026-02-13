using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class ProductAsyncControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        
        public ProductAsyncControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
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
        public async Task GetAll_Should_Return_Ok()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateProduct_Should_Return_Created()
        {
            await AuthenticateAsync();
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var request = new CreateProductRequest
            {
                Price = 100,
                IsActive = true,
                ProviderId = providerId
            };

            var response = await _client.PostAsJsonAsync("/api/products", request);
            
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task CreateProduct_Should_Return_BadRequest_When_Invalid()
        {
            await AuthenticateAsync();
            var request = new CreateProductRequest
            {
                Price = -10, // Invalid
                IsActive = true,
                ProviderId = Guid.NewGuid()
            };

            var response = await _client.PostAsJsonAsync("/api/products", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProduct_Should_Return_Ok_When_Exists()
        {
            await AuthenticateAsync();
            // Create first
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var request = new CreateProductRequest { Price = 50, IsActive = true, ProviderId = providerId };
            var createResponse = await _client.PostAsJsonAsync("/api/products", request);
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            var response = await _client.GetAsync($"/api/products/{id}");
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal(50, product.Price);
        }

        [Fact]
        public async Task GetProduct_Should_Return_NotFound_When_Missing()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_Should_Return_NoContent()
        {
            await AuthenticateAsync();
            // Create
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createResponse = await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 10, IsActive = true, ProviderId = providerId });
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            // Update
            var updateRequest = new UpdateProductRequest { Price = 20, IsActive = false };
            var response = await _client.PutAsJsonAsync($"/api/products/{id}", updateRequest);
            
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify
            var getResponse = await _client.GetAsync($"/api/products/{id}");
            var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.Equal(20, product!.Price);
            Assert.False(product.IsActive);
        }

        [Fact]
        public async Task UpdateProduct_Should_Return_NotFound_When_Missing()
        {
            await AuthenticateAsync();
            var response = await _client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", new UpdateProductRequest { Price = 10 });
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_Should_Return_NoContent()
        {
            await AuthenticateAsync();
            // Create
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createResponse = await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 10, IsActive = true, ProviderId = providerId });
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            // Delete
            var response = await _client.DeleteAsync($"/api/products/{id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify
            var getResponse = await _client.GetAsync($"/api/products/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_Should_Return_NotFound_When_Missing()
        {
            await AuthenticateAsync();
            var response = await _client.DeleteAsync($"/api/products/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NoContent()
        {
            await AuthenticateAsync();
            // Create
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createResponse = await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 10, IsActive = true, ProviderId = providerId });
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            var response = await _client.PatchAsJsonAsync($"/api/products/{id}/price", new ChangePriceRequest { NewPrice = 99 });
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ApplyDiscount_Should_Return_NoContent()
        {
            await AuthenticateAsync();
            // Create
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createResponse = await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 100, IsActive = true, ProviderId = providerId });
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            var response = await _client.PatchAsync($"/api/products/{id}/discount?discount=10", null);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ActivateDesactivate_Should_Return_NoContent()
        {
            await AuthenticateAsync();
            // Create
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createResponse = await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 100, IsActive = true, ProviderId = providerId });
            var location = createResponse.Headers.Location?.ToString();
            var id = location!.Substring(location.LastIndexOf('/') + 1);

            // Desactivate
            var responseDes = await _client.PatchAsync($"/api/products/{id}/desactivate", null);
            Assert.Equal(HttpStatusCode.NoContent, responseDes.StatusCode);

            // Activate
            var responseAct = await _client.PatchAsync($"/api/products/{id}/activate", null);
            Assert.Equal(HttpStatusCode.NoContent, responseAct.StatusCode);
        }
    }
}
