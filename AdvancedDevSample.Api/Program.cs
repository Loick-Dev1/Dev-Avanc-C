using AdvancedDevSample.Api.Middlewarre;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Application.Interfaces.Security; // Add Interface namespace
using AdvancedDevSample.Domain.Interfaces.Products;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvancedDevSample.Domain.Interfaces.Customers;
using AdvancedDevSample.Domain.Interfaces.Providers;
using AdvancedDevSample.Infrastructure;
using AdvancedDevSample.Infrastructure.Repositories;
using AdvancedDevSample.Infrastructure.Security; // Add Implementation namespace
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Added
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    foreach (var xmlfile in Directory.GetFiles(basePath, "*.xml"))
    {
        options.IncludeXmlComments(xmlfile);
    }
    
    // Add Security Definition for Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
             new Microsoft.OpenApi.Models.OpenApiSecurityScheme
             {
                 Reference = new Microsoft.OpenApi.Models.OpenApiReference
                 {
                     Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             Array.Empty<string>()
        }
    });
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var jwtKey = jwtSettings["SecretKey"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "AdvancedDevSample",
        ValidAudience = jwtSettings["Audience"] ?? "AdvancedDevSampleUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Repositories
builder.Services.AddScoped<IProductRepository, EfProductRepository>(); 
builder.Services.AddScoped<IProductRepositoryAsync, EfProductRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>(); 
builder.Services.AddScoped<IOrderRepositoryAsync, EfOrderRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IProviderRepository, EfProviderRepository>();

// Services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProviderService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi(); // Removed
    app.UseSwagger();
    app.UseSwaggerUI(); // Need to check if UI supports Auth
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Verify order: AuthN before AuthZ
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

await app.RunAsync();


