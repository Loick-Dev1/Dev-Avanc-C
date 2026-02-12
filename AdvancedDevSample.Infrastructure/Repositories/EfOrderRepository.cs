using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvancedDevSample.Infrastructure.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfOrderRepository : IOrderRepository, IOrderRepositoryAsync
    {
        private static readonly ConcurrentDictionary<Guid, Order> _store = new();

        public Order? GetById(Guid id)
        {
            _store.TryGetValue(id, out var order);
            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            return _store.Values.ToList();
        }

        public void Add(Order order)
        {
            if (!_store.TryAdd(order.Id, order))
            {
                throw new InfrastructureException($"Une commande avec l'id {order.Id} existe déjà.");
            }
        }

        public void Update(Order order)
        {
            if (!_store.ContainsKey(order.Id))
            {
                throw new InfrastructureException("Commande introuvable pour mise à jour.");
            }
            _store[order.Id] = order;
        }

        public void Delete(Guid id)
        {
            if (!_store.TryRemove(id, out _))
            {
                throw new InfrastructureException("Commande introuvable pour suppression.");
            }
        }

        public void Save(Order order)
        {
             _store.AddOrUpdate(order.Id, order, (_, __) => order);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(GetById(id));
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await Task.FromResult(GetAll());
        }

        public async Task SaveAsync(Order order)
        {
            Save(order);
            await Task.CompletedTask;
        }
    }
}
