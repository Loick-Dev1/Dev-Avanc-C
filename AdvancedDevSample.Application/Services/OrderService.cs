using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Customers;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepositoryAsync _orderRepository;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderService(
            IOrderRepositoryAsync orderRepository, 
            IProductRepositoryAsync productRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(ToDto);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new ApplicationServiceException("Commande introuvable", System.Net.HttpStatusCode.NotFound);

            return ToDto(order);
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null) throw new ApplicationServiceException("Client introuvable", System.Net.HttpStatusCode.BadRequest);

            var order = new Order(request.CustomerId);

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new ApplicationServiceException($"Produit {item.ProductId} introuvable", System.Net.HttpStatusCode.BadRequest);

                if (!product.IsActive)
                     throw new ApplicationServiceException($"Produit {product.Id} inactif", System.Net.HttpStatusCode.BadRequest);

                order.AddItem(product, item.Quantity);
            }

            await _orderRepository.SaveAsync(order);
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
