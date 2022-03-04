using chattr.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace chattr.Server.Helpers
{
    public class JWTHelper
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JWTHelper> _logger;

        public JWTHelper(IConfiguration config, ILogger<JWTHelper> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateJsonWebToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(15), 
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = signingKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
