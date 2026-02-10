using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProviderRepository : IProviderRepository
    {
        private static readonly ConcurrentDictionary<Guid, Provider> _store = new();

        public EfProviderRepository()
        {
            // Seed some data
            var id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            _store.TryAdd(id, new Provider(id, "Acme Corp", "contact@acme.com", "123-456-7890"));
        }

        public Task<Provider> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var provider);
            return Task.FromResult(provider);
        }

        public Task<IEnumerable<Provider>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Provider>>(_store.Values);
        }

        public Task AddAsync(Provider provider)
        {
            _store.TryAdd(provider.Id, provider);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Provider provider)
        {
            _store[provider.Id] = provider;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _store.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
