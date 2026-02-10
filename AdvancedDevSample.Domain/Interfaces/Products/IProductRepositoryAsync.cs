using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Interfaces.Products
{
    public interface IProductRepositoryAsync
    {
        Task<Product> GetByIdAsync(Guid id);
        Task SaveAsync(Product product);
    }
}
