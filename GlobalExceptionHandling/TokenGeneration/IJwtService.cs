using System.Threading.Tasks;

namespace Middleware.TokenGeneration
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string firstName, string lastName);
        string GenerateResetPasswordToken(string email);
        bool ValidateToken(string token, out int userId, out string email);
        Task<bool> SendResetTokenByEmailAsync(string email, string token);
    }
}
