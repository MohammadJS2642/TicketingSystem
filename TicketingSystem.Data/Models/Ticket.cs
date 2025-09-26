using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Data.Models;

public class Ticket
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    [Required]
    public TicketStatus TicketStatus { get; set; } = TicketStatus.Open;

    [Required]
    public TicketPriority TicketPriority { get; set; } = TicketPriority.Low;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
}


public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Urgent
}