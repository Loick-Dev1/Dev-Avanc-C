using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Interfaces.Products
{
    public interface IProductRepository
    {
        Product? GetById(Guid id);
        IEnumerable<Product> GetAll();
        void Add(Product product);
        void Update(Product product);
        void Delete(Guid id);
        void Save(Product product); 
    }

 
}
