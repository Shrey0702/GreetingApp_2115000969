using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.DTO;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL: IUserBL
    {
        private readonly IUserRL _userRL;
        ILogger<UserBL> _logger;
        public UserBL(IUserRL userRL, ILogger<UserBL> logger)
        {
            _userRL = userRL;
            _logger = logger;
        }

        public AccountLoginResponse LoginUserBL(LoginDTO loginDTO)
        {
            _logger.LogInformation("Business Layer method called to login the user in database");
            var response = _userRL.LoginUserRL(loginDTO);
            return response;
        }

        public AccountLoginResponse RegisterUserBL(UserRegistrationModel newUser)
        {
            _logger.LogInformation("Business Layer method called to register the user in database");
            var response = _userRL.RegisterUserRL(newUser);
            return response;
        }
    }
}
