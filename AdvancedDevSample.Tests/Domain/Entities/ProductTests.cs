using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class ProductTest
    {

        [Fact]
        public void ChangePrice_Should_Update_Price_When_Product_Is_Active()
        {
            //Arrange : je prepare un produit valide
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid());
            product.ChangePrice(10);

            //Act : execute une action
            product.ChangePrice(20);

            //Assert : verification
            Assert.Equal(20, product.Price);
        }

        [Fact]
        public void ChangePrice_Should_Throw_Exception_When_Product_Is_Inactive()
        {
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid());
            product.ChangePrice(10); //valeur initiale

            typeof(Product).GetProperty(nameof(Product.IsActive))!.SetValue(product, false);

            var exception = Assert.Throws<DomainException>(() => product.ChangePrice(20));

            Assert.Equal("Produit Inactif", exception.Message);
        }

        [Fact]
        public void ApplyDiscount_Should_Decrease_Price()
        {
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid());
            product.ChangePrice(100);

            product.ApplyDiscount(30);

            Assert.Equal(70, product.Price);
        }

        [Fact]
        public void ApplyDiscount_Should_Throw_When_Resulting_Price_Is_Invalid()
        {
            var product = new Product(Guid.NewGuid(), 10m, true, Guid.NewGuid());
            product.ChangePrice(20);

            Assert.Throws<DomainException>(() => product.ApplyDiscount(30));
        }
    }
}