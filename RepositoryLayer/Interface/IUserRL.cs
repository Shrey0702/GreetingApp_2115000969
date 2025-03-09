using ModelLayer.DTO;
using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public AccountLoginResponse LoginUserRL(LoginDTO loginDTO);
        public AccountLoginResponse RegisterUserRL(UserRegistrationModel newUser);
    }
}
