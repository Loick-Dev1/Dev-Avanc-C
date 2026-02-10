using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Interfaces.Products
{
    public interface IProductRepositoryAsync
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task SaveAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
