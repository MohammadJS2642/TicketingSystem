using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TicketingSystem.Data.DTO;
using TicketingSystem.Data.Models;
using TicketingSystem.Infrastructure;

namespace TicketingSystem.Controllers;


[ApiController]
[Route("autn")]
public class AuthController(AppDbContext context, IConfiguration configuration) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Data.DTO.LoginRequest request)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var hash = new PasswordHasher<User>();
        var result = hash.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = GenerateJwtToke(user);
        return Ok(new LoginResponse(token));
    }

    private string GenerateJwtToke(User user)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key = jwtSection.GetValue<string>("Key");
        var issuer = jwtSection.GetValue<string>("Issuer");
        var audience = jwtSection.GetValue<string>("Audience");
        var expirationMinutes = jwtSection.GetValue<int>("ExpirationMinutes");

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
