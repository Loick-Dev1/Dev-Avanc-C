using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllAsync()
        {
            var providers = await _providerRepository.GetAllAsync();
            return providers.Select(p => new ProviderDto 
            { 
                Id = p.Id, 
                Name = p.Name, 
                Email = p.Email,
                ContactInfo = p.ContactInfo
            });
        }

        public async Task<Guid> CreateProviderAsync(CreateProviderRequest request)
        {
            var provider = new Provider(Guid.NewGuid(), request.Name, request.Email, request.ContactInfo);
            await _providerRepository.AddAsync(provider);
            return provider.Id;
        }

         public async Task<ProviderDto?> GetByIdAsync(Guid id)
        {
             var provider = await _providerRepository.GetByIdAsync(id);
            if (provider == null) return null;

            return new ProviderDto
            {
                Id = provider.Id,
                Name = provider.Name,
                Email = provider.Email,
                ContactInfo = provider.ContactInfo
            };
        }
    }
}
