using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Tests.Application.Fakes
{
    public class FakeProductRepository : IProductRepository
    {
        public bool WasSaved { get; private set; }

        private readonly Product _product;

        public FakeProductRepository(Product product)
        {
            _product = product;
        }

        public Product GetById(Guid id) => _product;

        public void Save(Product product)
        {
            WasSaved = true;
        }

        public IEnumerable<Product> GetAll()
        {
            return new List<Product> { _product };
        }

        public void Add(Product product)
        {
            // No-op for fake
        }

        public void Update(Product product)
        {
            // No-op for fake
        }

        public void Delete(Guid id)
        {
            // No-op for fake
        }
    }
}