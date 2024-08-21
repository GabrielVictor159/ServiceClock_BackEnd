
using Microsoft.IdentityModel.Tokens;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceClock_BackEnd.Infraestructure.Services;

public class TokenService : ITokenService
{
    public string Generate(string rule, Guid IdUser)
    {
        var Secret = Environment.GetEnvironmentVariable("JWTSECRET");
        if (Secret == null)
        {
            throw new InvalidOperationException("SECRET environment variable not deified");
        }
        var tokenExpire = Environment.GetEnvironmentVariable("TOKEN_EXPIRES");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("User_Rule", rule),
                        new Claim("User_Id", IdUser.ToString())
                }),
            Expires = DateTime.UtcNow.AddHours(tokenExpire != null ? int.Parse(tokenExpire) : 8),
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenFinal = tokenHandler.WriteToken(token);
        return tokenFinal;
    }
}

