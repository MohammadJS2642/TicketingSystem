using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Data.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Fullname { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public Role Role { get; set; }
    public ICollection<Ticket>? CreatedTicket { get; set; }
    public ICollection<Ticket>? AssignedTicket { get; set; }
}


public enum Role
{
    Admin,
    Employee
}
