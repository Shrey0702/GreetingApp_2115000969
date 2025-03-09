using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Middleware.TokenGeneration
{
    public interface IJwtService
    {
        string GenerateToken(string email, string firstName, string lastName);
        string GenerateResetPasswordToken(string email);
        bool ValidateToken(string token, out string email);
    }
}
