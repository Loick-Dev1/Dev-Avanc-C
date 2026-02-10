using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public Order()
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantité invalide");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.AddQuantity(quantity);
            }
            else
            {
                _items.Add(new OrderItem(product.Id, product.Price, quantity));
            }

            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(i => i.Price * i.Quantity);
        }
    }
}
