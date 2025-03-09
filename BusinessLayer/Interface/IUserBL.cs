using ModelLayer.DTO;
using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public AccountLoginResponse LoginUserBL(LoginDTO loginDTO);
        public AccountLoginResponse RegisterUserBL(UserRegistrationModel newUser);
        public Task<bool> ForgetPasswordAsync(string email);
        public Task<bool> ResetPasswordAsync(string token, string newPassword);

    }
}
