using EthenAPI.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EthenAPI.Services
{
    public class JwtTokenService
    {
        public JwtTokenService()
        { 

        }
        public string GenerateToken(string userId, string user_email)
        {
            double TokenLifetime = 0;
            double.TryParse(Program.jwtSettings.TokenLifetimeMinutes, out TokenLifetime);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier,userId),
            new Claim(ClaimTypes.Email, user_email) ,           
            new Claim("SessionID", Guid.NewGuid().ToString())            
            };

            var token = new JwtSecurityToken(
                Program.jwtSettings.ValidIssuer,
                Program.jwtSettings.ValidAudience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(TokenLifetime),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }        
    }
}
