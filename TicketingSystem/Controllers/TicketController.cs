using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketingSystem.Application.DTO;
using TicketingSystem.Application.Interfacces;

namespace TicketingSystem.Controllers;

[ApiController]
[Route("tickets")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticketDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await ticketService.CreateTicketAsync(userId, ticketDto);
        return CreatedAtAction(nameof(GetTicketById), new { id = result.Id }, result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var tickets = await ticketService.GetMyTickets(userId);
        return Ok(tickets);
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTickets()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var tickets = await ticketService.GetTicketsAsync(userId);
        return Ok(tickets);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PUT(Guid id, [FromBody] UpdateTicketDto ticketDto)
    {
        var findedTicket = await ticketService.UpdateTicketAsync(id, ticketDto);
        if (findedTicket == null)
            return NotFound();

        return Ok(findedTicket);
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Stats()
    {
        var result = await ticketService.GetStats();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticketResult = await ticketService.GetTicketByIdAsync(id);
        if (ticketResult == null)
            return NotFound();

        return Ok(ticketResult);
    }
}
