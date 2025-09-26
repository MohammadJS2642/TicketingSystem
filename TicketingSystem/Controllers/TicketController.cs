using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketingSystem.Data;
using TicketingSystem.Data.DTO;
using TicketingSystem.Data.Models;

namespace TicketingSystem.Controllers;

[ApiController]
[Route("tickets")]
public class TicketController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticketDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
        return Ok(tickets);
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTickets()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
        return Ok(tickets);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PUT(Guid id, [FromBody] UpdateTicketDto ticketDto)
    {
        var findedTicket = await context.Tickets.FindAsync(id);
        if (findedTicket == null)
            return NotFound();

        findedTicket.TicketStatus = ticketDto.TicketStatus;
        findedTicket.AssignedToUserId = ticketDto.AssignedToUserId;

        context.Tickets.Update(findedTicket);
        await context.SaveChangesAsync();
        return Ok(findedTicket);
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Stats()
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
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket == null)
            return NotFound();

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
        return Ok(ticketDetails);
    }
}
