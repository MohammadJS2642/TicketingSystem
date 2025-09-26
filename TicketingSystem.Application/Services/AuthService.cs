using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TicketingSystem.Application.DTO;
using TicketingSystem.Application.Interfacces;
using TicketingSystem.Data.Models;
using TicketingSystem.Infrastructure;

namespace TicketingSystem.Application.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await context.Users
           .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return null;
        }

        var hash = new PasswordHasher<User>();
        var result = hash.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = GenerateJwtToke(user);
        return new LoginResponse(token);
    }

    private string GenerateJwtToke(User user)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expirationMinutes = int.Parse(jwtSection["ExpirationMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new (ClaimTypes.Name,user.Fullname),
            new (ClaimTypes.Email,user.Email),
            new (ClaimTypes.Role,user.Role.ToString())
        };

        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
