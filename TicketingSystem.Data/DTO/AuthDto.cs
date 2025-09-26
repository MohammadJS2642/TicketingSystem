namespace TicketingSystem.Data.DTO;

public record LoginRequest(string email, string password);
public record LoginResponse(string token, string tokenType = "Bearer");