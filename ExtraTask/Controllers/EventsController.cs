using ExtaTask.Exceptions;
using ExtaTask.Models;
using ExtaTask.Models.Dto;
using ExtaTask.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExtaTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEventsAsync()
    {
        return Ok(await service.GetCurrEventsAsync());
    }

    [HttpDelete("cancel")]
    public async Task<IActionResult> CancelEventsAsync([FromQuery] int participantId, [FromQuery] int eventId)
    {
        try
        {
            await service.DeleteRegistrationAsync(participantId, eventId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("assign-speaker")]
    public async Task<IActionResult> AssignSpeaker([FromQuery] int speakerId, [FromQuery] int eventId)
    {
        try
        {
            await service.AssignSpeakerToEventAsync(speakerId, eventId);
            return Ok("Speaker assigned successfully");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpPost("create-event")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
    {
        try
        {
            var createdEvent = await service.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("events/{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var ev = await service.GetEventByIdAsync(id);
        if (ev is null)
            return NotFound();

        return Ok(ev);
    }
}