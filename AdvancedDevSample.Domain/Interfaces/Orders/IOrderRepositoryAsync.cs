using AdvancedDevSample.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Interfaces.Orders
{
    public interface IOrderRepositoryAsync
    {
        Task<Order> GetByIdAsync(Guid id);
        Task SaveAsync(Order order);
    }
}
