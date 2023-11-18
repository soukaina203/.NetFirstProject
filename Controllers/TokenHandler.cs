using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Providers
{
     public class TokenHandler
    {
        private readonly AppSettings _appSettings;
        public TokenHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateTokken(Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            Console.Write( tokenHandler);
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var createToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(createToken);
        }
    }
}