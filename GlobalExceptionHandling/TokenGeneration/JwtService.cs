using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.TokenGeneration
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates a JWT Token for authentication.
        /// </summary>
        public string GenerateToken(string email, string firstName, string lastName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, $"{firstName} {lastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a JWT Token for Reset Password functionality.
        /// </summary>
        public string GenerateResetPasswordToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Shorter expiry for security
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validates the Reset Password Token and extracts the email.
        /// </summary>
        public bool ValidateToken(string token, out string email)
        {
            email = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var emailClaim = principal.FindFirst(ClaimTypes.Email);
                email = emailClaim?.Value;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sends a Reset Password Token to the user via email.
        /// </summary>
        public async Task<bool> SendResetTokenByEmailAsync(string email, string token)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your App", _config["SMTP:SenderEmail"]));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Password Reset Request";

                message.Body = new TextPart("plain")
                {
                    Text = $"Hello,\n\nClick the link below to reset your password:\n" +
                           $"{_config["App:ResetPasswordUrl"]}?token={token}\n\nThis link will expire in 15 minutes."
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_config["SMTP:Host"], int.Parse(_config["SMTP:Port"]), true);
                    await client.AuthenticateAsync(_config["SMTP:Username"], _config["SMTP:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
