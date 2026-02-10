using AdvancedDevSample.Domain.Exceptions;
using System;

namespace AdvancedDevSample.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public OrderItem(Guid productId, decimal price, int quantity)
        {
            if (quantity <= 0) throw new DomainException("Quantité invalide");
            if (price < 0) throw new DomainException("Prix invalide");

            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            if (quantity <= 0) throw new DomainException("Quantité doit être positive");
            Quantity += quantity;
        }
    }
}
