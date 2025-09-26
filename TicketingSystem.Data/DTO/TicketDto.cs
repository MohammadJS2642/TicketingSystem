using TicketingSystem.Data.Models;

namespace TicketingSystem.Data.DTO;

public record CreateTicketDto(string title, string? description, TicketPriority Priority = TicketPriority.Low);

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