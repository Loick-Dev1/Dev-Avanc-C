using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions; // Added
using AdvancedDevSample.Application.Interfaces.Security;
using AdvancedDevSample.Domain.Entities; // Added
using AdvancedDevSample.Domain.Interfaces.Customers;
using System;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class AuthService
    {
        private readonly ICustomerRepository _customerRepository; // Or dedicated IUserRepo
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(ICustomerRepository customerRepository, IJwtTokenGenerator tokenGenerator)
        {
            _customerRepository = customerRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // For this sample, we just verify email exists. In real app, check password hash.
            // Using GetAllAsync filtering logic would be inefficient in prod, but fine for sample/in-memory.
            // Ideally: GetByEmailAsync
            // Let's iterate for now.
            
            var customers = await _customerRepository.GetAllAsync();
            var user = customers.FirstOrDefault(c => c.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                 throw new ApplicationServiceException("Utilisateur inconnu", System.Net.HttpStatusCode.Unauthorized);
            }

            // Simulating password check (always valid if user exists for this demo, or check hardcoded pwd)
            if (request.Password != "password") // Hardcoded check just to have some logic
            {
                 throw new ApplicationServiceException("Mot de passe incorrect", System.Net.HttpStatusCode.Unauthorized);
            }

            var token = _tokenGenerator.GenerateToken(user.Id, user.Email, "Customer");

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email
            };
        }
    }
}
