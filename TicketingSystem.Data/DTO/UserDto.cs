namespace TicketingSystem.Data.DTO;

public record UserDto(Guid Id, string Fullname, string Email, string Role);