using AdvancedDevSample.Domain.Exceptions;
using System;

namespace AdvancedDevSample.Domain.Entities
{
    public class Provider
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string ContactInfo { get; private set; }

        public Provider(Guid id, string name, string email, string contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");
            
            Id = id;
            Name = name;
            Email = email;
            ContactInfo = contactInfo;
        }

        public void UpdateInfo(string name, string email, string contactInfo)
        {
             if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");

            Name = name;
            Email = email;
            ContactInfo = contactInfo;
        }
    }
}
