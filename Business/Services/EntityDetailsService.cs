using Business.Dtos;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class EventDetailsService(DataContext context) : IEventDetailsService
{
    private readonly DataContext _context = context;

    public async Task<EventDetailsDto> CreateEventDetailsAsync(EventDetailsDto eventDetailsDto)
    {
        try
        {
            var existingDetails = await _context.EventDetails
                .FirstOrDefaultAsync(e => e.EventId == eventDetailsDto.EventId);

            if (existingDetails != null)
            {
                throw new Exception($"Event details already exist for event ID {eventDetailsDto.EventId}");
            }

            var eventDetailsEntity = new EventDetailsEntity
            {
                EventId = eventDetailsDto.EventId,
                Description = eventDetailsDto.Description,
                TotalTickets = eventDetailsDto.TotalTickets,
                TicketsLeft = eventDetailsDto.TotalTickets
            };

            await _context.EventDetails.AddAsync(eventDetailsEntity);
            await _context.SaveChangesAsync();

            return MapToEventDetailsDto(eventDetailsEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create event details: {ex.Message}", ex);
        }
    }

    public async Task<EventDetailsDto?> GetEventDetailsByEventIdAsync(string eventId)
    {
        try
        {
            var eventDetails = await _context.EventDetails
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventDetails == null) return null;

            return MapToEventDetailsDto(eventDetails);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get event details for event ID {eventId}: {ex.Message}", ex);
        }
    }

    public async Task UpdateEventDetailsByEventIdAsync(string eventId, EventDetailsDto eventDetailsDto)
    {
        try
        {
            var eventDetailsEntity = await _context.EventDetails
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventDetailsEntity == null)
                throw new Exception($"Event details for event ID {eventId} not found");

            eventDetailsEntity.Description = eventDetailsDto.Description;
            eventDetailsEntity.TotalTickets = eventDetailsDto.TotalTickets;
            eventDetailsEntity.TicketsLeft = eventDetailsDto.TicketsLeft;

            _context.EventDetails.Update(eventDetailsEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update event details: {ex.Message}", ex);
        }
    }

    public async Task<int> GetTicketsLeftAsync(string eventId)
    {
        try
        {
            var eventDetails = await _context.EventDetails
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            return eventDetails?.TicketsLeft ?? 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get tickets left for event ID {eventId}: {ex.Message}", ex);
        }
    }

    public async Task UpdateTicketsLeftAsync(string eventId, int newTicketsLeft)
    {
        try
        {
            var eventDetails = await _context.EventDetails
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventDetails == null)
                throw new Exception($"Event details for event ID {eventId} not found");

            eventDetails.TicketsLeft = newTicketsLeft;

            _context.EventDetails.Update(eventDetails);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update tickets left for event ID {eventId}: {ex.Message}", ex);
        }
    }

    private static EventDetailsDto MapToEventDetailsDto(EventDetailsEntity entity)
    {
        return new EventDetailsDto
        {
            Id = entity.Id,
            EventId = entity.EventId,
            Description = entity.Description,
            TotalTickets = entity.TotalTickets,
            TicketsLeft = entity.TicketsLeft
        };
    }
}