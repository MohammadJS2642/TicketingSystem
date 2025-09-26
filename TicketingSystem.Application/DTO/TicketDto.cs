using TicketingSystem.Data.Models;

namespace TicketingSystem.Application.DTO;

public record CreateTicketDto(string Title, string? Description, TicketPriority Priority = TicketPriority.Low);

public record UpdateTicketDto(TicketStatus TicketStatus, Guid? AssignedToUserId);

public record TicketDetailsDto(
    Guid Id,
    string Title,
    string? Description,
    TicketStatus Status,
    TicketPriority Priority,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid CreatedByUserId,
    Guid? AssignedToUserId
);