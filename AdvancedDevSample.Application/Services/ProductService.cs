using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        // CRUD publics
        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _repository.GetAll();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public ProductDto GetProductById(Guid productId)
        {
            var product = GetProduct(productId);
            return new ProductDto
            {
                Id = product.Id,
                Price = product.Price,
                IsActive = product.IsActive
            };
        }

        public Guid CreateProduct(CreateProductRequest request)
        {
            // validation par attributs attendue côté controller, ici création de l'entité
            var product = new Product(Guid.NewGuid(), request.Price, request.IsActive);
            _repository.Add(product);
            return product.Id;
        }

        public void UpdateProduct(Guid productId, UpdateProductRequest request)
        {
            var product = GetProduct(productId);

            if (request.Price.HasValue)
            {
                // utilisation du comportement du domaine pour préserver les invariants
                product.ChangePrice(request.Price.Value);
            }

            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value) product.Activate();
                else product.Desactivate();
            }

            _repository.Update(product);
        }

        public void DeleteProduct(Guid productId)
        {
            // délégation au repository
            _repository.Delete(productId);
        }

        // Fonctions métier existantes
        public void ChangeProductPrice(Guid productId, decimal newPrice)    
        {
            var product = GetProduct(productId);
            product.ChangePrice(newPrice);
            _repository.Save(product);
        }

        public void ApplyProductDiscount(Guid productId, decimal discount)
        {
            var product = GetProduct(productId);
            product.Activate();
            product.ApplyDiscount(discount);
            _repository.Save(product);
        }

        public void ActivateProduct(Guid produtId)
        {
            var product = GetProduct(produtId);
            product.Activate();
            _repository.Save(product);
        }

        public void DesactivateProduct(Guid productId)
        {
            var product = GetProduct(productId);
            product.Desactivate();
            _repository.Save(product);
        }

        private Product GetProduct(Guid productId)
        {
            return _repository.GetById(productId)
                ?? throw new ApplicationServiceException("Produit introuvable", System.Net.HttpStatusCode.NotFound);
        }
    }
}