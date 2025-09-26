using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.DTO;
using TicketingSystem.Application.Interfacces;

namespace TicketingSystem.Controllers;


[ApiController]
[Route("autn")]
public class AuthController(IAuthService auth) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var authService = await auth.LoginAsync(request);
        if (auth == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(authService);
    }

}
