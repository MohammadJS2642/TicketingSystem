using TicketingSystem.Application.DTO;
using TicketingSystem.Data.Models;

namespace TicketingSystem.Application.Interfacces;

public interface ITicketService
{
    Task<Ticket> CreateTicketAsync(Guid userId, CreateTicketDto ticketDto);
    Task<IEnumerable<TicketDetailsDto>> GetTicketsAsync(Guid userId);
    Task<Ticket?> UpdateTicketAsync(Guid id, UpdateTicketDto ticketDto);
    Task<TicketDetailsDto?> GetTicketByIdAsync(Guid id);
    Task<object> GetStats();
    Task<IEnumerable<TicketDetailsDto>> GetMyTickets(Guid userId);
}
