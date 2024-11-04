using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;

namespace ServiceClock_BackEnd.Authorization;

public static class TokenHandlerParameters
{
    public static TokenValidationParameters addjwtsecuritytokenhandlerparameters(this TokenValidationParameters jwtsecuritytokenhandler)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTSECRET")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}

