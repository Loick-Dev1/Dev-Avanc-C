using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace AdvancedDevSample.Tests.Integration
{
    public class OrdersApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public OrdersApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        {"JwtSettings:SecretKey", "super-secret-key-for-tests-only-1234567890"},
                        {"JwtSettings:Issuer", "AdvancedDevSample"},
                        {"JwtSettings:Audience", "AdvancedDevSampleUsers"}
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
        public async Task CreateOrder_Should_Return_Created_And_Location()
        {
            await AuthenticateAsync();

            // 1. Create Product
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createProductRequest = new CreateProductRequest
            {
                Price = 100,
                IsActive = true,
                ProviderId = providerId
            };
            var productResponse = await _client.PostAsJsonAsync("/api/products", createProductRequest);
            productResponse.EnsureSuccessStatusCode();
            var productLocation = productResponse.Headers.Location?.ToString();
            var productId = Guid.Parse(productLocation!.Substring(productLocation.LastIndexOf('/') + 1));

            // 2. Create Order
            var customerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var orderRequest = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 2 }
                }
            };

            var response = await _client.PostAsJsonAsync("/api/orders", orderRequest);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task GetAllOrders_Should_Return_Ok()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();
            Assert.NotNull(orders);
        }

        [Fact]
        public async Task GetOrder_Should_Return_Ok_For_Existing_Order()
        {
            await AuthenticateAsync();

            // 1. Create Product
            var providerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var createProductRequest = new CreateProductRequest
            {
                Price = 50,
                IsActive = true,
                ProviderId = providerId
            };
            var productResponse = await _client.PostAsJsonAsync("/api/products", createProductRequest);
            productResponse.EnsureSuccessStatusCode();
            var productLocation = productResponse.Headers.Location?.ToString();
            var productId = Guid.Parse(productLocation!.Substring(productLocation.LastIndexOf('/') + 1));

            // 2. Create Order
            var customerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var orderRequest = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 1 }
                }
            };

            var createOrderResponse = await _client.PostAsJsonAsync("/api/orders", orderRequest);
            createOrderResponse.EnsureSuccessStatusCode();
            var location = createOrderResponse.Headers.Location?.ToString();
            var orderId = Guid.Parse(location!.Substring(location.LastIndexOf('/') + 1));

            // 3. Get Order
            var response = await _client.GetAsync($"/api/orders/{orderId}");
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            Assert.NotNull(order);
            Assert.Equal(orderId, order.Id);
            Assert.NotEmpty(order.Items);
            Assert.Equal(50, order.TotalAmount);
        }
    }
}
