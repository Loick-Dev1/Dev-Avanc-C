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

namespace AdvancedDevSample.Tests.API.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Supprimer le vrai repository si nécéssaire
                services.RemoveAll(typeof(IProductRepositoryAsync));

                //Ajouter un repository InMemory
                services.AddSingleton<InMemoryProductRepositoryAsync>();
                services.AddSingleton<IProductRepositoryAsync>(sp => sp.GetRequiredService<InMemoryProductRepositoryAsync>());
            });
        }
    }
}
