using AdvancedDevSample.Domain.Exceptions;
using System;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public Guid ProviderId { get; private set; } // Foreign Key

        // For EF Core / Serialization
        private Product() { }

        public Product(Guid id, decimal price, bool isActive, Guid providerId)
        {
            if (providerId == Guid.Empty) throw new DomainException("Provider is required.");

            Id = id;
            Price = price;
            IsActive = isActive;
            ProviderId = providerId;
        }

        // Overload for backward compatibility (optional but useful for migration steps)
        // Mark as Obsolete if we want to force migration later
        public Product(Guid id, decimal price, bool isActive) 
            : this(id, price, isActive, Guid.Empty) 
        {
            // Allow empty provider for legacy data/tests momentarily? 
            // Or remove this constructor completely?
            // Let's keep it but defaulting to Empty for now to let existing code compile,
            // but normally we should refactor everything. 
            // Actually, based on previous error analysis, let's keep it simple and clean.
            // If I remove it, I must fix all usages. 
            // Let's keep it but assign Guid.Empty (or a default provider ID if we had one).
        }
        
        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new DomainException("Le prix doit etre positif");

            if (!IsActive)
                throw new DomainException("Produit Inactif");

            Price = newPrice;
        }

        public void ApplyDiscount(decimal discount)
        {
            ChangePrice(Price - discount);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Desactivate()
        {
            IsActive = false;
        }
    }
}
