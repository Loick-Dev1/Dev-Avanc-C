using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Interfaces.Providers
{
    public interface IProviderRepository
    {
        Task<Provider?> GetByIdAsync(Guid id);
        Task<IEnumerable<Provider>> GetAllAsync();
        Task AddAsync(Provider provider);
        Task UpdateAsync(Provider provider);
        Task DeleteAsync(Guid id);
    }
}
