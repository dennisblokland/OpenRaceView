using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenRaceView.Application.Commands.Laps;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Application.Queries.Laps;

namespace OpenRaceView.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LapsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LapsController> _logger;

    public LapsController(IMediator mediator, ILogger<LapsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CreateLapResponse>> CreateLap([FromBody] CreateLapRequest request)
    {
        try
        {
            var command = new CreateLapCommand(request);
            var result = await _mediator.Send(command);
            
            var locationUri = Url.Action(nameof(GetLap), new { id = result.Id });
            return Created(locationUri, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid lap creation request");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating lap");
            return StatusCode(500, new { error = "An error occurred while creating the lap" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<LapListItemDto>>> GetLaps()
    {
        try
        {
            var query = new GetLapListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving laps");
            return StatusCode(500, new { error = "An error occurred while retrieving laps" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LapDetailDto>> GetLap(Guid id, [FromQuery] bool includeSamples = false)
    {
        try
        {
            var query = new GetLapDetailQuery(id, includeSamples);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving lap {LapId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the lap" });
        }
    }
}