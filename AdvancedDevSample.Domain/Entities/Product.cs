using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        
            public Guid Id { get; private set; } //Identité
            public decimal Price { get; private set; }
            public bool IsActive { get; private set; } //false par défaut
            public Product()
            {
                IsActive = true;
            }
            public Product(Guid id, decimal price, bool isActive)
            {   
                Id = id;
                Price = price;
                IsActive = isActive;
            }

            public void ChangePrice(decimal newPrice) //Comportement
            { if (newPrice <= 0) // Invariant
                    throw new DomainException("Le prix doit etre positif");

                if (!IsActive) //Règle Métier
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
