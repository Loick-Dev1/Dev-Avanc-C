using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using System;
using Xunit;

namespace AdvancedDevSample.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void ChangePrice_Should_Update_Price()
        {
            var product = new Product(Guid.NewGuid(), 100m, true, Guid.NewGuid());
            product.ChangePrice(150m);
            Assert.Equal(150m, product.Price);
        }

        [Fact]
        public void ChangePrice_Should_Throw_If_Negative()
        {
            var product = new Product(Guid.NewGuid(), 100m, true, Guid.NewGuid());
            Assert.Throws<DomainException>(() => product.ChangePrice(-10m));
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True()
        {
            var product = new Product(Guid.NewGuid(), 100m, false, Guid.NewGuid());
            product.Activate();
            Assert.True(product.IsActive);
        }

        [Fact]
        public void Desactivate_Should_Set_IsActive_False()
        {
            var product = new Product(Guid.NewGuid(), 100m, true, Guid.NewGuid());
            product.Desactivate();
            Assert.False(product.IsActive);
        }

        [Fact]
        public void ApplyDiscount_Should_Lower_Price()
        {
            var product = new Product(Guid.NewGuid(), 100m, true, Guid.NewGuid());
            product.ApplyDiscount(20m); // 20% discount
            Assert.Equal(80m, product.Price);
        }

        [Fact]
        public void ApplyDiscount_Should_Throw_If_Invalid_Range()
        {
            var product = new Product(Guid.NewGuid(), 100m, true, Guid.NewGuid());
            Assert.Throws<DomainException>(() => product.ApplyDiscount(-1m));
            Assert.Throws<DomainException>(() => product.ApplyDiscount(101m));
        }
    }
}