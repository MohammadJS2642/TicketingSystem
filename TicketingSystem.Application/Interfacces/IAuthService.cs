using TicketingSystem.Application.DTO;

namespace TicketingSystem.Application.Interfacces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
