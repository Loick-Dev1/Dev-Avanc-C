using AdvancedDevSample.Domain.Interfaces.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Supprimer le vrai repository si nécéssaire
                services.RemoveAll<IProductRepositoryAsync>();

                //Ajouter un repository InMemory
                services.AddSingleton<InMemoryProductRepositoryAsync>();
                services.AddSingleton<IProductRepositoryAsync>(sp => sp.GetRequiredService<InMemoryProductRepositoryAsync>());
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"JwtSettings:SecretKey", "super-secret-key-for-tests-only-1234567890"},
                    {"JwtSettings:Issuer", "AdvancedDevSample"},
                    {"JwtSettings:Audience", "AdvancedDevSampleUsers"}
                });
            });
        }
    }
}
