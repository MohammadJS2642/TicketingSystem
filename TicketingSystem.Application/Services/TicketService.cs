using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.DTO;
using TicketingSystem.Application.Interfacces;
using TicketingSystem.Data.Models;
using TicketingSystem.Infrastructure;

namespace TicketingSystem.Application.Services;

public class TicketService(AppDbContext context) : ITicketService
{
    public async Task<Ticket> CreateTicketAsync(Guid userId, CreateTicketDto ticketDto)
    {
        var ticket = new Ticket
        {
            Title = ticketDto.Title,
            Description = ticketDto.Description,
            TicketPriority = ticketDto.Priority,
            TicketStatus = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            CreatedByUserId = userId,
            AssignedToUserId = null // اول هیچکس assign نشده
        };

        context.Tickets.Add(ticket);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
        return ticket;
    }

    public async Task<TicketDetailsDto?> GetTicketByIdAsync(Guid id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket == null)
            return null;

        var ticketDetails = new TicketDetailsDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.TicketStatus,
            ticket.TicketPriority,
            ticket.CreatedAt,
            ticket.UpdateAt,
            ticket.CreatedByUserId,
            ticket.AssignedToUserId
        );
        return ticketDetails;
    }

    public async Task<IEnumerable<TicketDetailsDto>> GetTicketsAsync(Guid userId)
    {
        try
        {
            var tickets = await context.Tickets
               .Select(t => new TicketDetailsDto(
                   t.Id,
                   t.Title,
                   t.Description,
                   t.TicketStatus,
                   t.TicketPriority,
                   t.CreatedAt,
                   t.UpdateAt,
                   t.CreatedByUserId,
                   t.AssignedToUserId
               )).ToListAsync();
            return tickets;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Ticket?> UpdateTicketAsync(Guid id, UpdateTicketDto ticketDto)
    {
        var findedTicket = await context.Tickets.FindAsync(id);
        if (findedTicket == null)
            return null;

        findedTicket.TicketStatus = ticketDto.TicketStatus;
        findedTicket.AssignedToUserId = ticketDto.AssignedToUserId;
        context.Tickets.Update(findedTicket);
        await context.SaveChangesAsync();

        return findedTicket;
    }

    public async Task<object> GetStats()
    {
        var ticketsCount = await context.Tickets.CountAsync();

        var ticketsStatus = await context.Tickets
            .GroupBy(t => t.TicketStatus)
            .Select(ts => new
            {
                Status = ts.Key.ToString(),
                Count = ts.Count()
            }).ToListAsync();
        var result = new
        {
            Total = ticketsCount,
            TicketsStatus = ticketsStatus
        };
        return result;
    }

    public async Task<IEnumerable<TicketDetailsDto>> GetMyTickets(Guid userId)
    {
        var tickets = await context.Tickets
            .Where(t => t.CreatedByUserId == userId)
            .Select(t => new TicketDetailsDto(
                t.Id,
                t.Title,
                t.Description,
                t.TicketStatus,
                t.TicketPriority,
                t.CreatedAt,
                t.UpdateAt,
                t.CreatedByUserId,
                t.AssignedToUserId
            )).ToListAsync();
        return tickets;
    }
}
