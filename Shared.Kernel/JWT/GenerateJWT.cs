using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace Shared.Kernel.JWT
{
    public class JWTService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public JWTService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> GenerateJWT(string username, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var userId = user.Id;
            var secretKeyString = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(secretKeyString))
            {
                throw new InvalidOperationException("La clave secreta de JWT no está configurada.");
            }

            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("user_id", userId.ToString()),
                new Claim("username", username),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}