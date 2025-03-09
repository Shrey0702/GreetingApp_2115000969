using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        ILogger<AccountController> _logger;
        private readonly IUserBL _userBL;
        public AccountController(ILogger<AccountController> logger, IUserBL userBL)
        {
            _logger = logger;
            _userBL = userBL;

        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get Greeting method called");
            ResponseModel<string> response = new ResponseModel<string>
            {

                Data = ""
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult RegisterUser(UserRegistrationModel newUser)
        {
            _logger.LogInformation("Post method called to register the user in database");
            var response = _userBL.RegisterUserBL(newUser);
            AccountResponse<AccountLoginResponse> registerResponse = new AccountResponse<AccountLoginResponse>
            {
                Data = new AccountLoginResponse
                {
                    Message = response.Message,
                    Success = response.Success,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Email = response.Email
                }
            };
            return Ok(registerResponse);
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser(LoginDTO loginDTO)
        {
            _logger.LogInformation("Post method called to login the user in database");
            var response = _userBL.LoginUserBL(loginDTO);
            ResponseModel<AccountLoginResponse> loginResponse = new ResponseModel<AccountLoginResponse>
            {
                Data = new AccountLoginResponse
                {
                    Message = response.Message,
                    Success = response.Success,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Email = response.Email,
                    Token = response.Token
                }
            };
            return Ok(loginResponse);
        }
    }
}
