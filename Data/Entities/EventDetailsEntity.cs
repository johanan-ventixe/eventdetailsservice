using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class EventDetailsEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string EventId { get; set; } = null!;

    public string? Description { get; set; }

    public int TotalTickets { get; set; }

    public int TicketsLeft { get; set; }
}