using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto 
            { 
                Id = c.Id, 
                FirstName = c.FirstName, 
                LastName = c.LastName, 
                Email = c.Email 
            });
        }

        public async Task<Guid> CreateCustomerAsync(CreateCustomerRequest request)
        {
            var customer = new Customer(Guid.NewGuid(), request.FirstName, request.LastName, request.Email);
            await _customerRepository.AddAsync(customer);
            return customer.Id;
        }

        public async Task<CustomerDto?> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
        }
    }
}
