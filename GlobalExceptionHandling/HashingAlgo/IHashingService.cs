using System;

namespace Middleware.HashingAlgo
{
    public interface IHashingService
    {
        string HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHash);
    }
}
