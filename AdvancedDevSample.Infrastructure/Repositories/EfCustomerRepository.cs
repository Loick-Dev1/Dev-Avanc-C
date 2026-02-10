using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Customers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfCustomerRepository : ICustomerRepository
    {
        private static readonly ConcurrentDictionary<Guid, Customer> _store = new();

        public EfCustomerRepository()
        {
            // Seed some data
            var id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            _store.TryAdd(id, new Customer(id, "John", "Doe", "john.doe@example.com"));
        }

        public Task<Customer> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var customer);
            return Task.FromResult(customer);
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Customer>>(_store.Values);
        }

        public Task AddAsync(Customer customer)
        {
            _store.TryAdd(customer.Id, customer);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Customer customer)
        {
            _store[customer.Id] = customer;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _store.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
