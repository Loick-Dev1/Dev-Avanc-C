using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using AdvancedDevSample.Domain.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepositoryAsync _repository;
        private readonly IProviderRepository _providerRepository;

        public ProductService(IProductRepositoryAsync repository, IProviderRepository providerRepository)
        {
            _repository = repository;
            _providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Price = p.Price,
                IsActive = p.IsActive,
                ProviderId = p.ProviderId
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await GetProduct(productId);
            return new ProductDto
            {
                Id = product.Id,
                Price = product.Price,
                IsActive = product.IsActive,
                ProviderId = product.ProviderId
            };
        }

        public async Task<Guid> CreateProductAsync(CreateProductRequest request)
        {
            var provider = await _providerRepository.GetByIdAsync(request.ProviderId);
            if (provider == null) throw new ApplicationServiceException("Fournisseur introuvable", System.Net.HttpStatusCode.BadRequest);

            var product = new Product(Guid.NewGuid(), request.Price, request.IsActive, request.ProviderId);
            await _repository.SaveAsync(product);
            return product.Id;
        }

        public async Task UpdateProductAsync(Guid productId, UpdateProductRequest request)
        {
            var product = await GetProduct(productId);

            if (request.Price.HasValue)
            {
                product.ChangePrice(request.Price.Value);
            }

            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value) product.Activate();
                else product.Desactivate();
            }

            await _repository.SaveAsync(product);
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null) throw new ApplicationServiceException("Produit introuvable", System.Net.HttpStatusCode.NotFound);
            await _repository.DeleteAsync(productId);
        }

        // Domain logic wrappers
        public async Task ChangeProductPriceAsync(Guid productId, decimal newPrice)    
        {
            var product = await GetProduct(productId);
            product.ChangePrice(newPrice);
            await _repository.SaveAsync(product);
        }

        public async Task ApplyProductDiscountAsync(Guid productId, decimal discount)
        {
            var product = await GetProduct(productId);
            product.Activate(); // Ensure active before discount? or leave as is?
            product.ApplyDiscount(discount);
            await _repository.SaveAsync(product);
        }

        public async Task ActivateProductAsync(Guid produtId)
        {
            var product = await GetProduct(produtId);
            product.Activate();
            await _repository.SaveAsync(product);
        }

        public async Task DesactivateProductAsync(Guid productId)
        {
            var product = await GetProduct(productId);
            product.Desactivate();
            await _repository.SaveAsync(product);
        }

        private async Task<Product> GetProduct(Guid productId)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null) throw new ApplicationServiceException("Produit introuvable", System.Net.HttpStatusCode.NotFound);
            return product;
        }
    }
}