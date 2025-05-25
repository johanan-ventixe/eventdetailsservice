using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventDetailsController(IEventDetailsService eventDetailsService) : ControllerBase
{
    private readonly IEventDetailsService _eventDetailsService = eventDetailsService;

    [HttpPost]
    public async Task<ActionResult<EventDetailsDto>> CreateEventDetails(EventDetailsDto eventDetailsDto)
    {
        try
        {
            var createdEventDetails = await _eventDetailsService.CreateEventDetailsAsync(eventDetailsDto);
            return CreatedAtAction(nameof(GetEventDetailsByEventId),
                new { eventId = createdEventDetails.EventId }, createdEventDetails);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("already exist"))
            {
                return Conflict(ex.Message);
            }
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<EventDetailsDto>> GetEventDetailsByEventId(string eventId)
    {
        try
        {
            var eventDetails = await _eventDetailsService.GetEventDetailsByEventIdAsync(eventId);
            if (eventDetails == null)
            {
                return NotFound($"No details found for event ID {eventId}");
            }
            return Ok(eventDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("event/{eventId}")]
    public async Task<IActionResult> UpdateEventDetailsByEventId(string eventId, EventDetailsDto eventDetailsDto)
    {
        try
        {
            var existingDetails = await _eventDetailsService.GetEventDetailsByEventIdAsync(eventId);

            if (existingDetails == null)
            {
                return NotFound($"No event details found for event ID {eventId}");
            }

            eventDetailsDto.Id = existingDetails.Id;
            eventDetailsDto.EventId = eventId;

            await _eventDetailsService.UpdateEventDetailsByEventIdAsync(eventId, eventDetailsDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("event/{eventId}/tickets")]
    public async Task<ActionResult<int>> GetTicketsLeft(string eventId)
    {
        try
        {
            var eventDetails = await _eventDetailsService.GetEventDetailsByEventIdAsync(eventId);
            if (eventDetails == null)
            {
                return NotFound($"No details found for event ID {eventId}");
            }
            return Ok(eventDetails.TicketsLeft);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPatch("event/{eventId}/tickets/{newCount}")]
    public async Task<IActionResult> UpdateTicketsCount(string eventId, int newCount)
    {
        try
        {
            var eventDetails = await _eventDetailsService.GetEventDetailsByEventIdAsync(eventId);
            if (eventDetails == null)
            {
                return NotFound($"No details found for event ID {eventId}");
            }

            eventDetails.TicketsLeft = newCount;
            await _eventDetailsService.UpdateEventDetailsByEventIdAsync(eventId, eventDetails);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}