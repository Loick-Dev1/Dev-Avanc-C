using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Interfaces.Orders
{
    public interface IOrderRepositoryAsync
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task SaveAsync(Order order);
    }
}
