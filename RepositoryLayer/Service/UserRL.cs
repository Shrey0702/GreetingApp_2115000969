using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Middleware.HashingAlgo;
using Middleware.TokenGeneration;
using ModelLayer.DTO;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingAppContext _userDbContext;
        private readonly ILogger<UserRL> _logger;
        private readonly IHashingService _hashingService;
        private readonly IJwtService _jwtService;

        public UserRL(GreetingAppContext userDbContext, ILogger<UserRL> logger, IHashingService hashingService, IJwtService jwtService)
        {
            _userDbContext = userDbContext;
            _logger = logger;
            _hashingService = hashingService;
            _jwtService = jwtService;
        }

        public AccountLoginResponse LoginUserRL(LoginDTO loginDTO)
        {
            _logger.LogInformation($"Attempting login for user: {loginDTO.Email}");

            var user = _userDbContext.UserEntities
                .FirstOrDefault(user => user.Email.ToLower() == loginDTO.Email.ToLower());

            if (user == null)
            {
                _logger.LogError($"Login failed: User not found ({loginDTO.Email})");
                return new AccountLoginResponse
                {
                    Message = "User Not Found in the database.",
                    Success = false,
                    FirstName = "not applicable",
                    LastName = "not applicable",
                    Email = "not applicable"
                };
            }

            if (!_hashingService.VerifyPassword(loginDTO.Password, user.PasswordHash))
            {
                _logger.LogError($"Login failed: Incorrect password for {loginDTO.Email}");
                return new AccountLoginResponse
                {
                    Message = "Password is incorrect, Please try again :)",
                    Success = false,
                    FirstName = "not applicable",
                    LastName = "not applicable",
                    Email = "not applicable"
                };
            }

            _logger.LogInformation($"User {loginDTO.Email} logged in successfully.");
            var token = _jwtService.GenerateToken(user.Email, user.FirstName, user.LastName);
            return new AccountLoginResponse
            {
                Message = "User Logged in Successfully",
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token
            };
        }

        public AccountLoginResponse RegisterUserRL(UserRegistrationModel newUser)
        {
            _logger.LogInformation($"Attempting registration for user: {newUser.Email}");

            var existingUser = _userDbContext.UserEntities
                .FirstOrDefault(user => user.Email.ToLower() == newUser.Email.ToLower());

            if (existingUser != null)
            {
                _logger.LogError($"Registration failed: Email already exists ({newUser.Email})");
                return new AccountLoginResponse
                {
                    Message = "User Already Exists in the database. please try to login",
                    Success = false,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email
                };
            }

            var hashedPassword = _hashingService.HashPassword(newUser.Password);

            UserEntity userEntity = new UserEntity
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email.ToLower(),
                PasswordHash = hashedPassword
            };

            _userDbContext.UserEntities.Add(userEntity);
            _userDbContext.SaveChanges();

            _logger.LogInformation($"User {newUser.Email} registered successfully.");

            return new AccountLoginResponse
            {
                Message = "User Registered Successfully",
                Success = true,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email
            };
        }
    }
}
