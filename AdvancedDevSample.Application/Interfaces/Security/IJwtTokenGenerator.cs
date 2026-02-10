using System;
using System.Security.Claims;

namespace AdvancedDevSample.Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, string role);
    }
}
