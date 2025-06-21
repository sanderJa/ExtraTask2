using ExtaTask.Exceptions;
using ExtaTask.Models.Dto;
using ExtaTask.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExtaTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllParticipants()
    {
        var participants = await service.GetAllParticipantsAsync();
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetParticipant(int id)
    {
        try
        {
            return Ok(await service.GetParticipantAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateParticipantAsync([FromBody] ParticipantCreateDto dto)
    {
        var id = await service.CreateParticipantAsync(dto);
        return CreatedAtAction(nameof(GetParticipant), new { id }, dto);
    }
}