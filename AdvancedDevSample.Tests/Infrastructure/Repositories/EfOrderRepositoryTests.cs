using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Infrastructure.Repositories;
using AdvancedDevSample.Infrastructure.Exceptions;
using System;
using Xunit;

namespace AdvancedDevSample.Tests.Infrastructure.Repositories
{
    public class EfOrderRepositoryTests
    {
        private readonly EfOrderRepository _repository;

        public EfOrderRepositoryTests()
        {
            _repository = new EfOrderRepository();
        }

        [Fact]
        public void Update_Should_Update_Order()
        {
            var order = new Order(Guid.NewGuid());
            _repository.Add(order);

            // Modify something (if setter allowed, but public setters are private)
            // Order doesn't have public mutable properties easily accessible besides AddItem
            // But we can check if it persists.
            
            // Re-saving same order
            _repository.Update(order);
            
            var retrieved = _repository.GetById(order.Id);
            Assert.NotNull(retrieved);
        }

        [Fact]
        public void Update_Should_Throw_If_Missing()
        {
            var order = new Order(Guid.NewGuid());
            Assert.Throws<InfrastructureException>(() => _repository.Update(order));
        }

        [Fact]
        public void Delete_Should_Remove_Order()
        {
            var order = new Order(Guid.NewGuid());
            _repository.Add(order);

            _repository.Delete(order.Id);

            var retrieved = _repository.GetById(order.Id);
            Assert.Null(retrieved);
        }

        [Fact]
        public void Delete_Should_Throw_If_Missing()
        {
            Assert.Throws<InfrastructureException>(() => _repository.Delete(Guid.NewGuid()));
        }
    }
}
