using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public TokenGenerator(string issuerSigningKey)
        {
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
        }

        public string CreateToken(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    }
                ),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}