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

namespace AdvancedDevSample.Tests.Integration
{
    public class CustomerProviderApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CustomerProviderApiTests(WebApplicationFactory<Program> factory)
        {
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
        public async Task GetAllCustomers_ShouldReturnOk_And_IncludeSeededData()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync("/api/customers");
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerDto>>();
            Assert.NotNull(customers);
            Assert.NotEmpty(customers); // Should contain the seeded "John Doe"
            Assert.Contains(customers, c => c.Email == "john.doe@example.com");
        }

        [Fact]
        public async Task GetCustomer_Should_Return_Ok_When_Exists()
        {
            await AuthenticateAsync();
            var seededId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var response = await _client.GetAsync($"/api/customers/{seededId}");
            response.EnsureSuccessStatusCode();
            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            Assert.Equal(seededId, customer!.Id);
        }

        [Fact]
        public async Task GetCustomer_Should_Return_NotFound_When_Missing()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync($"/api/customers/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnCreated()
        {
            await AuthenticateAsync();
            var request = new CreateCustomerRequest 
            { 
                FirstName = "Jane", 
                LastName = "Doe", 
                Email = "jane.doe@example.com" 
            };
            var response = await _client.PostAsJsonAsync("/api/customers", request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task CreateCustomer_Should_Return_BadRequest_When_Invalid()
        {
            await AuthenticateAsync();
            var request = new CreateCustomerRequest 
            { 
                FirstName = "", // Invalid
                LastName = "Doe", 
                Email = "invalid-email" 
            };
            var response = await _client.PostAsJsonAsync("/api/customers", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProviders_ShouldReturnOk_And_IncludeSeededData()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync("/api/providers");
            response.EnsureSuccessStatusCode();
            var providers = await response.Content.ReadFromJsonAsync<IEnumerable<ProviderDto>>();
            Assert.NotNull(providers);
            Assert.NotEmpty(providers); // Should contain seeded "Acme Corp"
        }

        [Fact]
        public async Task GetProvider_Should_Return_Ok_When_Exists()
        {
            await AuthenticateAsync();
            var seededId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var response = await _client.GetAsync($"/api/providers/{seededId}");
            response.EnsureSuccessStatusCode();
            var provider = await response.Content.ReadFromJsonAsync<ProviderDto>();
            Assert.Equal(seededId, provider!.Id);
        }

        [Fact]
        public async Task GetProvider_Should_Return_NotFound_When_Missing()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync($"/api/providers/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateProvider_ShouldReturnCreated()
        {
            await AuthenticateAsync();
            var request = new CreateProviderRequest 
            { 
                Name = "Globex Corp", 
                Email = "contact@globex.com",
                ContactInfo = "555-1234"
            };
            var response = await _client.PostAsJsonAsync("/api/providers", request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task CreateProvider_Should_Return_BadRequest_When_Invalid()
        {
            await AuthenticateAsync();
            var request = new CreateProviderRequest 
            { 
                Name = "", // Invalid
                Email = "bad-email",
                ContactInfo = ""
            };
            var response = await _client.PostAsJsonAsync("/api/providers", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
