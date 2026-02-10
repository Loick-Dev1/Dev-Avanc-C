using AdvancedDevSample.Domain.Exceptions;
using System;

namespace AdvancedDevSample.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public Customer(Guid id, string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public void UpdateProfile(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
