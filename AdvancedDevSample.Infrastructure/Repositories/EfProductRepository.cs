using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using AdvancedDevSample.Infrastructure.Entities;
using AdvancedDevSample.Infrastructure.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository, IProductRepositoryAsync
    {
        // Simule une table en mémoire. Remplacez par DbContext/EF Core plus tard.
        private static readonly ConcurrentDictionary<Guid, ProductEntity> _store = new();

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(GetById(id));
        }

        public async Task SaveAsync(Product product)
        {
            Save(product);
            await Task.CompletedTask;
        }

        public Product? GetById(Guid id)
        {
            try
            {
                if (_store.TryGetValue(id, out var entity))
                {
                    return ToDomain(entity);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération du produit.", ex);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                return _store.Values.Select(ToDomain).ToList();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la liste des produits.", ex);
            }
        }

        public void Add(Product product)
        {
            try
            {
                var entity = ToEntity(product);
                if (!_store.TryAdd(entity.Id, entity))
                {
                    throw new InfrastructureException($"Un produit avec l'id {entity.Id} existe déjà.");
                }
            }
            catch (InfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de l'ajout du produit.", ex);
            }
        }

        public void Update(Product product)
        {
            try
            {
                var entity = ToEntity(product);
                if (!_store.ContainsKey(entity.Id))
                {
                    throw new InfrastructureException("Produit introuvable pour mise à jour.");
                }

                _store[entity.Id] = entity;
            }
            catch (InfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la mise à jour du produit.", ex);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                if (!_store.TryRemove(id, out _))
                {
                    throw new InfrastructureException("Produit introuvable pour suppression.");
                }
            }
            catch (InfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la suppression du produit.", ex);
            }
        }

        public void Save(Product product)
        {
            try
            {
                var entity = ToEntity(product);
                // Upsert
                _store.AddOrUpdate(entity.Id, entity, (_, __) => entity);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la sauvegarde du produit.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(GetAll());
        }

        public async Task DeleteAsync(Guid id)
        {
            Delete(id);
            await Task.CompletedTask;
        }

        // Helpers de mapping
        private static Product ToDomain(ProductEntity e) =>
            new Product(id: e.Id, price: e.Price, isActive: e.IsActive, providerId: e.ProviderId);

        private static ProductEntity ToEntity(Product p) =>
            new ProductEntity { Id = p.Id, Price = p.Price, IsActive = p.IsActive, ProviderId = p.ProviderId };
    }
}
