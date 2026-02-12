using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace AdvancedDevSample.Tests.Integration
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        {"JwtSettings:SecretKey", "super-secret-key-for-tests-only-1234567890"}
                    });
                });
            }).CreateClient();
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
        public async Task CreateProduct_Should_Return_Created_And_Location()
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
        public async Task GetProduct_Should_Return_Product_Start_With_Empty()
        {
             var response = await _client.GetAsync("/api/products");
             response.EnsureSuccessStatusCode();
             var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
             Assert.NotNull(products);
        }
        
        [Fact]
        public async Task ChangePrice_Async_Should_Update_Price()
        {
            await AuthenticateAsync();
            // 1. Create Product
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createRequest = new CreateProductRequest { Price = 50, IsActive = true, ProviderId = providerId };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.EnsureSuccessStatusCode();
            
            // Extract ID from Location header
            var location = createResponse.Headers.Location?.ToString();
            Assert.NotNull(location);
            var idString = location!.Substring(location.LastIndexOf('/') + 1);
            var id = Guid.Parse(idString);

            // 2. Change Price Async
            var priceRequest = new ChangePriceRequest { NewPrice = 75 };
            var updateResponse = await _client.PatchAsJsonAsync($"/api/products/{id}/price", priceRequest);
            updateResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // 3. Verify
            var getResponse = await _client.GetAsync($"/api/products/{id}");
            var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal(75, product.Price);
        }
    }
}
