namespace Business.Dtos;

public class EventDetailsDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventId { get; set; } = null!;
    public string? Description { get; set; }
    public int TotalTickets { get; set; }
    public int TicketsLeft { get; set; }
}