using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using System;
using Xunit;

namespace AdvancedDevSample.Tests.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_Should_Recalculate_TotalAmount()
        {
            // Arrange
            var order = new Order();
            var product = new Product(Guid.NewGuid(), 100m, true);

            // Act
            order.AddItem(product, 2);

            // Assert
            Assert.Equal(200m, order.TotalAmount);
            Assert.Single(order.Items);
        }

        [Fact]
        public void AddItem_Should_Update_Quantity_If_Already_Exists()
        {
            // Arrange
            var order = new Order();
            var product = new Product(Guid.NewGuid(), 100m, true);
            order.AddItem(product, 1);

            // Act
            order.AddItem(product, 2);

            // Assert
            Assert.Equal(300m, order.TotalAmount);
            Assert.Single(order.Items); // Toujours 1 item, mais quantité augmentée
        }

        [Fact]
        public void AddItem_Should_Throw_If_Quantity_Invalid()
        {
            var order = new Order();
            var product = new Product(Guid.NewGuid(), 100m, true);

            Assert.Throws<DomainException>(() => order.AddItem(product, 0));
        }
    }
}
