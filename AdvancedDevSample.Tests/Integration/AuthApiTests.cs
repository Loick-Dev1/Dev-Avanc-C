using AdvancedDevSample.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace AdvancedDevSample.Tests.Integration
{
    public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthApiTests(WebApplicationFactory<Program> factory)
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

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            var loginRequest = new LoginRequest { Email = "john.doe@example.com", Password = "password" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            response.EnsureSuccessStatusCode();
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            Assert.NotNull(authResponse);
            Assert.False(string.IsNullOrEmpty(authResponse.Token));
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
        {
            var loginRequest = new LoginRequest { Email = "john.doe@example.com", Password = "wrongpassword" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithUnknownUser_ShouldReturnUnauthorized()
        {
            var loginRequest = new LoginRequest { Email = "unknown@example.com", Password = "password" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
