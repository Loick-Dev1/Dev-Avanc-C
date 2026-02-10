using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Domain.Interfaces.Orders
{
    public interface IOrderRepository
    {
        Order GetById(Guid id);
        IEnumerable<Order> GetAll();
        void Add(Order order);
        void Update(Order order);
        void Delete(Guid id);
        void Save(Order order);
    }
}
