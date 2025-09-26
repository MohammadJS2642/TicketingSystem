using Microsoft.AspNetCore.Identity;
using TicketingSystem.Data.Models;

namespace TicketingSystem.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Users.Any())
            return;

        var hash = new PasswordHasher<User>();

        var admin = new User
        {
            Fullname = "Admin",
            Email = "admin@gmail.com",
            Role = Role.Admin
        };
        admin.PasswordHash = hash.HashPassword(admin, "Admin@123");

        var employee = new User
        {
            Fullname = "Employee",
            Email = "employee@gmail.com",
            Role = Role.User
        };
        employee.PasswordHash = hash.HashPassword(employee, "Employee@123");

        context.Users.Add(admin);
        context.Users.Add(employee);


        var ticket = new Ticket
        {
            Title = "tmp",
            Description = "Sample",
            CreatedByUserId = employee.Id,
            TicketPriority = TicketPriority.Medium,
            TicketStatus = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };
        context.Tickets.Add(ticket);

        context.SaveChanges();
    }
}