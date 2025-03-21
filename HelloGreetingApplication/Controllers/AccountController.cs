﻿using BusinessLayer.Interface;
using BusinessLayer.Service;
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
        private readonly IRabbitMQService _rabbitMQService;
        public AccountController(ILogger<AccountController> logger, IUserBL userBL, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            _userBL = userBL;
            _rabbitMQService = rabbitMQService;
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
            _rabbitMQService.SendMessage($"{newUser.Email}, You have successfully Registered!");
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
            _rabbitMQService.ReceiveMessage();
            return Ok(loginResponse);
        }

        /// <summary>
        /// Sends password reset email
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            _logger.LogInformation($"Received password reset request for email: {model.Email ?? "NULL"}");

            _logger.LogInformation($"Received password reset request for email: {model.Email}");

            var result = await _userBL.ForgetPasswordAsync(model.Email);
            if (result)
                return Ok(new { Message = "Password reset email sent successfully." });

            return BadRequest(new { Message = "Email not found in the system." });
        }

        /// <summary>
        /// Resets password using token
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            _logger.LogInformation($"Processing password reset request for token: {model.Token}");

            var result = await _userBL.ResetPasswordAsync(model.Token, model.NewPassword);
            if (result)
                return Ok(new { Message = "Password reset successful." });

            return BadRequest(new { Message = "Invalid or expired token." });
        }
    }
}
