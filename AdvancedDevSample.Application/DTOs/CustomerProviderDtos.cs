using System;

namespace AdvancedDevSample.Application.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }

    public class CreateCustomerRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }

    public class ProviderDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string ContactInfo { get; set; }
    }

    public class CreateProviderRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string ContactInfo { get; set; }
    }
}
