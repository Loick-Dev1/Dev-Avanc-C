using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace AdvancedDevSample.Tests.Integration
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_Should_Return_Created_And_Location()
        {
            var request = new CreateProductRequest
            {
                Price = 100,
                IsActive = true
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
            // 1. Create Product
            var createRequest = new CreateProductRequest { Price = 50, IsActive = true };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.EnsureSuccessStatusCode();
            
            // Extract ID from Location header
            var location = createResponse.Headers.Location.ToString();
            var idString = location.Substring(location.LastIndexOf('/') + 1);
            var id = Guid.Parse(idString);

            // 2. Change Price Async
            var priceRequest = new ChangePriceRequest { NewPrice = 75 };
            var updateResponse = await _client.PutAsJsonAsync($"/api/products/productasync/{id}/price", priceRequest);
            updateResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // 3. Verify
            var getResponse = await _client.GetAsync($"/api/products/{id}");
            var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.Equal(75, product.Price);
        }
    }
}
