using ExtaTask.Models.Dto;
using ExtaTask.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExtaTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllSpeakers()
    {
        var speakers = await service.GetAllSpeakersAsync();
        return Ok(speakers);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSpeakerAsync([FromBody] SpeakerCreateDto dto)
    {
        var id = await service.CreateSpeakerAsync(dto);
        return CreatedAtAction(nameof(GetSpeaker), new { id }, dto);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpeaker(int id)
    {
        var speaker = await service.GetSpeakerAsync(id);

        if (speaker == null)
            return NotFound();

        return Ok(speaker);
    }
}