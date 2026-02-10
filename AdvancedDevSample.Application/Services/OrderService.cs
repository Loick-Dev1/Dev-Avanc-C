using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public IEnumerable<OrderDto> GetAllOrders()
        {
            var orders = _orderRepository.GetAll();
            return orders.Select(ToDto);
        }

        public OrderDto GetOrderById(Guid id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
                throw new ApplicationServiceException("Commande introuvable", System.Net.HttpStatusCode.NotFound);

            return ToDto(order);
        }

        public Guid CreateOrder(CreateOrderRequest request)
        {
            var order = new Order();

            foreach (var item in request.Items)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product == null)
                    throw new ApplicationServiceException($"Produit {item.ProductId} introuvable", System.Net.HttpStatusCode.BadRequest);

                if (!product.IsActive)
                     throw new ApplicationServiceException($"Produit {product.Id} inactif", System.Net.HttpStatusCode.BadRequest);

                order.AddItem(product, item.Quantity);
            }

            _orderRepository.Add(order);
            return order.Id;
        }

        private static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                })
            };
        }
    }
}
