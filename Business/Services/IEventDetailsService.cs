using Business.Dtos;

namespace Business.Services;

public interface IEventDetailsService
{
    Task<EventDetailsDto> CreateEventDetailsAsync(EventDetailsDto eventDetailsDto);
    Task<EventDetailsDto?> GetEventDetailsByEventIdAsync(string eventId);
    Task UpdateEventDetailsByEventIdAsync(string eventId, EventDetailsDto eventDetailsDto);
    Task<int> GetTicketsLeftAsync(string eventId);
    Task UpdateTicketsLeftAsync(string eventId, int newTicketsLeft);
}