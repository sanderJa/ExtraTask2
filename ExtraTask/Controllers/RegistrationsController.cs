using ExtaTask.Exceptions;
using ExtaTask.Models;
using ExtaTask.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExtaTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationsController(IDbService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> CreateRegistration([FromQuery] int parId,[FromQuery] int evId)
    {
        try
        {
            var id = await service.CreateRegistrationAsync(parId, evId);
            return CreatedAtAction(nameof(GetRegistration), new { id }, id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (AlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (FullEventException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRegistration(int id)
    {
        var reg = await service.GetRegistrationByIdAsync(id);
        if (reg is null)
            return NotFound();

        return Ok(reg);
    }
}